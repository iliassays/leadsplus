namespace Agent.Commands
{
    using Agent.Command;
    using Agent.Domain;
    using Agent.Domain.Query;
    using Agent.EmailCreator;
    using Agent.Services;
    using Agent.TypeFormIntegration;
    using Google.Apis.Sheets.v4.Data;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Extension;
    using LeadsPlus.Core.Query;
    using LeadsPlus.GoogleApis.Command;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Driver;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    
    public class AgentTypeFormAccountCommandHandler
        : IRequestHandler<CreateAgentTypeFormAccountForBuyInquiryCommand, bool>,
        IRequestHandler<CreateAgentTypeFormAccountForRentInquiryCommand, bool>
    {
        private readonly IEventBus eventBus;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IQueryExecutor queryExecutor;
        private readonly IRepository<Agent> agentRepository;
        private readonly ITypeForm typeForm;
        private readonly IConfiguration configuration;

        public AgentTypeFormAccountCommandHandler(IMediator mediator, 
            IRepository<Agent> agentRepository, 
            IEventBus eventBus, 
            IIdentityService identityService,
            IQueryExecutor queryExecutor,
            ITypeForm typeForm,
            IConfiguration configuration)
        {
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.typeForm = typeForm ?? throw new ArgumentNullException(nameof(typeForm));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(CreateAgentTypeFormAccountForBuyInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            //create buyer inquiry template
            var buyInquiryTemplateUrl = configuration.GetValue<string>("TypeFormBuyInquiryTemplateUrl");

            string typeFormTemplateJson = await typeForm.GetTypeFormAsync(buyInquiryTemplateUrl);

            var typeFormUrl = await CreateTypeFormUrl(agent, InquiryType.BuyInquiry, typeFormTemplateJson);

            if (agent.BuyInquiry == null)
            {
                agent.BuyInquiry = new AgentBuyInquiry();
            }

            agent.BuyInquiry.UpdateTypeform(typeFormUrl);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);

            var update = Builders<Agent>.Update
                .Set("BuyInquiry", agent.BuyInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateTypeform(InquiryType.BuyInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(CreateAgentTypeFormAccountForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            var rentInquiryTemplateUrl = configuration.GetValue<string>("TypeFormRentInquiryTemplateUrl");

            string typeFormTemplateJson = await typeForm.GetTypeFormAsync(rentInquiryTemplateUrl);

            var typeFormUrl = await CreateTypeFormUrl(agent, InquiryType.RentInquiry, typeFormTemplateJson);

            if (agent.RentInquiry == null)
            {
                agent.RentInquiry = new AgentRentInquiry();
            }

            agent.RentInquiry.UpdateTypeform(typeFormUrl);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);

            var update = Builders<Agent>.Update
                .Set("RentInquiry", agent.RentInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateTypeform(InquiryType.RentInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        private async Task<string> CreateTypeFormUrl(Agent agent, InquiryType typeformType, string typeFormTemplateJson)
        {
            Regex reg = new Regex(@"(\""title\"":[ ]?\"")([\d\s\w]*)");

            typeFormTemplateJson = reg.Replace(typeFormTemplateJson, delegate (Match m)
            {
                return m.Groups[1].Value + agent.GetTypeformName(Enum.GetName(typeof(InquiryType), typeformType));
            }, 1);

            var cretedTypeFormJson = await typeForm.CreateTypeFormAsync(typeFormTemplateJson);
            dynamic cretedTypeForm = JObject.Parse(cretedTypeFormJson);

            return cretedTypeForm._links.display;
        }
    }
}