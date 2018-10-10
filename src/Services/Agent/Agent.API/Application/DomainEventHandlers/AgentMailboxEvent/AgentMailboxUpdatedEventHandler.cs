namespace Agent.DomainEventHandlers
{
    using Agent.Domain.Events;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using LeadsPlus.Core;
    using Agent.Services;
    using Agent.Domain;
    using Agent.TypeFormIntegration;
    using Agent.Domain.Query;
    using MongoDB.Driver;
    using Agent.EmailCreator;

    public class AgentMailboxUpdatedEventHandler : INotificationHandler<AgentMailboxUpdatedEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<Agent> agentRepository;

        public AgentMailboxUpdatedEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<Agent> agentRepository)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
        }

        public async Task Handle(AgentMailboxUpdatedEvent agentMailboxUpdatedEvent, CancellationToken cancellationToken)
        {
            string mailboxName = agentMailboxUpdatedEvent.NewMailbox;

            var integrationEmail = await CreateIntigrationEmail(mailboxName);
            agentMailboxUpdatedEvent.Agent.IntegrationEmail = integrationEmail;

            var filter = Builders<Agent>.Filter.Eq("Id", agentMailboxUpdatedEvent.Agent.Id);
            var update = Builders<Agent>.Update
                .Set("IntegrationEmail", integrationEmail)
                .CurrentDate("UpdatedDate");

            await agentRepository.Collection
                .UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });

            logger.CreateLogger(nameof(agentMailboxUpdatedEvent)).LogTrace($"Agent mailbox created for {agentMailboxUpdatedEvent.Agent.Id}.");
        }

        public async Task<string> CreateIntigrationEmail(string mailboxName)
        {
            var emailAccount = new EmailAccount
            {
                Domain = "adfenixleads.com",//should come from config
                UserName = mailboxName,
                Password = "changeme",
                Quota = 500
            };

            return await new MailboxCreator(emailAccount).Create();
        }
    }
}
