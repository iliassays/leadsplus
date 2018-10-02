namespace Contact.DomainEventHandlers.ContactCreatedEvent
{
    using Agent.Command;
    using Agent.Domain;
    using Agent.Domain.Events;
    using Agent.IntegrationEvents;
    using Agent.Services;
    using Agent.TypeFormIntegration;
    using Google.Apis.Sheets.v4.Data;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.GoogleApis.Command;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class AgentSpreadsheetCreatedDomainEventHandler
                        : INotificationHandler<AgentSpreadsheetCreatedEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<Agent> agentRepository;
        private readonly IMediator madiator;

        public AgentSpreadsheetCreatedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<Agent> agentRepository,
            IMediator madiator)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.agentRepository = agentRepository ?? throw new ArgumentNullException(nameof(agentRepository));
            this.madiator = madiator ?? throw new ArgumentNullException(nameof(madiator));
        }

        public async Task Handle(AgentSpreadsheetCreatedEvent agentSpreadsheetCreatedEvent, CancellationToken cancellationToken)
        {
            //Do create typeform
            logger.CreateLogger(nameof(agentSpreadsheetCreatedEvent)).LogTrace($"Agent spreadsheet created {agentSpreadsheetCreatedEvent.Agent.Id}. Typeform: {Enum.GetName(typeof(global::Agent.Domain.InquiryType), agentSpreadsheetCreatedEvent.TypeFormType)}");
        }
    }
}
