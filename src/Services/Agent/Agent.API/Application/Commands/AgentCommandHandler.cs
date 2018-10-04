namespace Agent.Commands
{
    using Agent.Command;
    using Agent.Domain;
    using Agent.Domain.Query;
    using Agent.EmailCreator;
    using Agent.Services;
    using Agent.TypeFormIntegration;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Extension;
    using LeadsPlus.Core.Query;
    using MediatR;
    using MongoDB.Driver;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;
    
    public class AgentCommandHandler
        : IRequestHandler<CreateAgentCommand, bool>, 
        IRequestHandler<UpdateAgentCommand, bool>,
        IRequestHandler<DeleteAgentCommand, bool>,
        IRequestHandler<CreateAgentIntigrationEmailAccountCommand, bool>,
        IRequestHandler<UpdateAgentIntigrationEmailAccountCommand, bool>,
        IRequestHandler<UpdateAgentDataStudioUrlCommand, bool>,
        IRequestHandler<UpdateAgentAutoresponderTemplateForBuyInquiryCommand, bool>,
        IRequestHandler<UpdateAgentAutoresponderTemplateForRentInquiryCommand, bool>
    {
        private readonly IEventBus eventBus;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IQueryExecutor queryExecutor;
        private readonly IRepository<Agent> agentRepository;

        public AgentCommandHandler(IMediator mediator, 
            IRepository<Agent> agentRepository, 
            IEventBus eventBus, 
            IIdentityService identityService,
            IQueryExecutor queryExecutor)
        {
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));

            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CreateAgentCommand createAgentCommand, CancellationToken cancellationToken)
        {
            createAgentCommand.OwnerId = identityService.GetUserIdentity();

            var agent = new Agent(createAgentCommand.AggregateId,
                createAgentCommand.OwnerId,
                createAgentCommand.Firstname,
                createAgentCommand.Lastname,
                createAgentCommand.Email,
                createAgentCommand.Country,
                createAgentCommand.City,
                createAgentCommand.Phone,
                createAgentCommand.Address,
                createAgentCommand.Company);

            await agentRepository.AddAsync(agent);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(UpdateAgentCommand updateAgentCommand, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = updateAgentCommand.AggregateId });

            agent = DataMapper.Map<Agent, UpdateAgentCommand>(updateAgentCommand);

            //agent.Firstname = updateAgentCommand.Firstname;
            //agent.Lastname = updateAgentCommand.Lastname;
            //agent.City = updateAgentCommand.City;
            //agent.Email = updateAgentCommand.Email;
            //agent.Phone = updateAgentCommand.Phone;
            //agent.Country = updateAgentCommand.Country;
            //agent.Address = updateAgentCommand.Address;
            //agent.Company = updateAgentCommand.Company;

            agent.UpdatedDate = DateTime.UtcNow;

            await agentRepository.Collection.ReplaceOneAsync(
                doc => doc.Id == agent.Id,
                agent,
                new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<bool> Handle(DeleteAgentCommand deleteAgentCommand, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = deleteAgentCommand.AggregateId });

            await agentRepository.DeleteAsync(agent, deleteAgentCommand.AggregateId);

            return true;
        }

        public async Task<bool> Handle(CreateAgentIntigrationEmailAccountCommand createAgentIntigrationEmailAccountCommand, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = createAgentIntigrationEmailAccountCommand.AggregateId });

            agent.CreateMailbox();
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(UpdateAgentIntigrationEmailAccountCommand updateAgentIntigrationEmailAccountCommand, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = updateAgentIntigrationEmailAccountCommand.AggregateId });

            agent.UpdateMailbox(updateAgentIntigrationEmailAccountCommand.MailboxName);
            await mediator.DispatchDomainEventsAsync(agent);

            return true;
        }

        public async Task<bool> Handle(UpdateAgentDataStudioUrlCommand createAgentDataStudioUrlCommand, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = createAgentDataStudioUrlCommand.AggregateId });
            agent.DataStudioUrl = createAgentDataStudioUrlCommand.DataStudioUrl;

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);
            var update = Builders<Agent>.Update
                .Set("DataStudioUrl", agent.DataStudioUrl)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<bool> Handle(UpdateAgentAutoresponderTemplateForBuyInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            agent.BuyInquiry.InquiryAutoresponderTemplate.AgentAutoresponderTemplateId = @command.AgentAutoresponderTemplateForBuyInquiryId;
            agent.BuyInquiry.InquiryAutoresponderTemplate.CustomerAutoresponderTemplateId = @command.CustomerAutoresponderTemplateForBuyInquiryId;
            
            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);

            var update = Builders<Agent>.Update
                .Set("BuyInquiry", agent.BuyInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }

        public async Task<bool> Handle(UpdateAgentAutoresponderTemplateForRentInquiryCommand @command, CancellationToken cancellationToken)
        {
            var agent = await queryExecutor.Execute<GetAgentQuery, Agent>(new GetAgentQuery() { AgentId = @command.AggregateId });

            agent.RentInquiry.InquiryAutoresponderTemplate.AgentAutoresponderTemplateId = @command.AgentAutoresponderTemplateForRentInquiryId;
            agent.RentInquiry.InquiryAutoresponderTemplate.CustomerAutoresponderTemplateId = @command.CustomerAutoresponderTemplateForRentInquiryId;

            var filter = Builders<Agent>.Filter.Eq("Id", agent.Id);

            var update = Builders<Agent>.Update
                .Set("RentInquiry", agent.RentInquiry)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            return true;
        }
    }
}