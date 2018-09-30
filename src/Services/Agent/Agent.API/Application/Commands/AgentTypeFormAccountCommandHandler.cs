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
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    
    public class AgentTypeFormAccountCommandHandler
        : IRequestHandler<CreateAgentTypeFormAccountForBuyInquiryCommand, bool>,
        IRequestHandler<CreateAgentTypeFormAccountForRentInquiryCommand, bool>,
        IRequestHandler<CreateAgentSpreadsheetAccountForBuyInquiryCommand, bool>,
        IRequestHandler<CreateAgentSpreadsheetAccountForRentInquiryCommand, bool>
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

            var typeFormUrl = await CreateTypeFormUrl(agent, TypeFormType.BuyInquiry, typeFormTemplateJson);

            agent.BuyInquiryTypeForm = new AgentTypeForm(typeFormUrl, TypeFormType.BuyInquiry);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);

            var update = Builders<Agent>.Update
                .Set("BuyInquiryTypeForm", agent.BuyInquiryTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateTypeform(TypeFormType.BuyInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(CreateAgentTypeFormAccountForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            var rentInquiryTemplateUrl = configuration.GetValue<string>("TypeFormRentInquiryTemplateUrl");

            string typeFormTemplateJson = await typeForm.GetTypeFormAsync(rentInquiryTemplateUrl);

            var typeFormUrl = await CreateTypeFormUrl(agent, TypeFormType.BuyInquiry, typeFormTemplateJson);

            agent.RentInquiryTypeForm = new AgentTypeForm(typeFormUrl, TypeFormType.BuyInquiry);

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);

            var update = Builders<Agent>.Update
                .Set("RentInquiryTypeForm", agent.RentInquiryTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateTypeform(TypeFormType.RentInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(CreateAgentSpreadsheetAccountForBuyInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            //create buyer inquiry template
            var typeFormBuyInquiryTemplateUrl = configuration.GetValue<string>("TypeFormBuyInquiryTemplateUrl");

            string typeFormTemplateJson = await typeForm.GetTypeFormAsync(typeFormBuyInquiryTemplateUrl);

            var spreadsheet = CreateSpreadsheetForTypeform(agent, typeFormTemplateJson, TypeFormType.BuyInquiry);

            agent.BuyInquiryTypeForm.SpreadsheetUrl = spreadsheet.SpreadsheetUrl;
            agent.BuyInquiryTypeForm.SpreadsheetId = spreadsheet.SpreadsheetId;
            agent.BuyInquiryTypeForm.SpreadsheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(TypeFormType), TypeFormType.BuyInquiry));


            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("BuyInquiryTypeForm", agent.BuyInquiryTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateSpreadsheet(TypeFormType.BuyInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(CreateAgentSpreadsheetAccountForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            var typeFormRentInquiryTemplateUrl = configuration.GetValue<string>("TypeFormRentInquiryTemplateUrl");

            string typeFormTemplateJson = await typeForm.GetTypeFormAsync(typeFormRentInquiryTemplateUrl);

            var spreadsheet = CreateSpreadsheetForTypeform(agent, typeFormTemplateJson, TypeFormType.RentInquiry);

            agent.RentInquiryTypeForm.SpreadsheetUrl = spreadsheet.SpreadsheetUrl;
            agent.RentInquiryTypeForm.SpreadsheetId = spreadsheet.SpreadsheetId;
            agent.RentInquiryTypeForm.SpreadsheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(TypeFormType), TypeFormType.RentInquiry));
            
            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("RentInquiryTypeForm", agent.RentInquiryTypeForm)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            agent.CreateSpreadsheet(TypeFormType.RentInquiry);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        private async Task<string> CreateTypeFormUrl(Agent agent, TypeFormType typeformType, string typeFormTemplateJson)
        {
            Regex reg = new Regex(@"(\""title\"":[ ]?\"")([\d\s\w]*)");

            typeFormTemplateJson = reg.Replace(typeFormTemplateJson, delegate (Match m)
            {
                return m.Groups[1].Value + agent.GetTypeformName(Enum.GetName(typeof(TypeFormType), typeformType));
            }, 1);

            var cretedTypeFormJson = await typeForm.CreateTypeFormAsync(typeFormTemplateJson);
            dynamic cretedTypeForm = JObject.Parse(cretedTypeFormJson);

            return cretedTypeForm._links.display;
        }

        private Spreadsheet CreateSpreadsheetForTypeform(Agent agent, string typeFormTemplateJson, TypeFormType typeFormType)
        {
            dynamic typeForm = JObject.Parse(typeFormTemplateJson);

            //spreadSheetCreator.CreateSpreadSheetPermission(spreadsheet.SpreadsheetId, "bintusays@gmail.com");
            //return spreadsheet.SpreadsheetUrl;

            CreateSpreadsheetCommand createSpreadsheetCommand = new CreateSpreadsheetCommand
            {
                SpreadSheetName = agent.GetSpreadsheetName(Enum.GetName(typeof(TypeFormType), typeFormType)),
                WorkSheetName = Enum.GetName(typeof(TypeFormType), typeFormType),
                ApplicationName = "LeadsPlus"
            };

            foreach (dynamic item in typeForm.fields)
            {
                createSpreadsheetCommand.HeaderValues.Add(item.title);
                createSpreadsheetCommand.InitialValues.Add("-");
            }

            var spreadsheet = mediator.Send(createSpreadsheetCommand).Result;

            AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionCommand = new AssigSpreadsheetPermissionCommand
            {
                Email = agent.Email,
                SpreadsheetId = spreadsheet.SpreadsheetId,
                ApplicationName = "LeadsPlus"
            };

            var assigSpreadsheetPermissionCommandResult = mediator.Send(assigSpreadsheetPermissionCommand).Result;

            AssigSpreadsheetPermissionCommand assigSpreadsheetPermissionToOrganizationCommand = new AssigSpreadsheetPermissionCommand
            {
                Email = "adfenixleads@gmail.com",
                SpreadsheetId = spreadsheet.SpreadsheetId,
                ApplicationName = "LeadsPlus"
            };

            var assigSpreadsheetPermissionToOrganizationCommandResult = mediator.Send(assigSpreadsheetPermissionToOrganizationCommand).Result;

            //var createContactIntegrationEvent = new CreateContactIntegrationEvent()
            //{
            //    AggregateId = agent.Id,
            //    Source = "AdfenixLeads",
            //    Email = agent.Email,
            //    OwnerId = agent.Id,
            //    Ownername = $"{agent.Firstname} {agent.Lastname}"
            //};

            //eventBus.Publish(createContactIntegrationEvent);

            return spreadsheet;
        }
        
    }
}