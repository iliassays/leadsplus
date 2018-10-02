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
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Threading.Tasks;

    public class AgentTypeformCreatedDomainEventHandler
                        : INotificationHandler<AgentTypeformCreatedEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<Agent> agentRepository;
        private readonly IMediator madiator;

        public AgentTypeformCreatedDomainEventHandler(
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

        public async Task Handle(AgentTypeformCreatedEvent agentTypeformCreatedEvent, CancellationToken cancellationToken)
        {
            //Do create typeform
            logger.CreateLogger(nameof(agentTypeformCreatedEvent)).LogTrace($"Agent typeform created {agentTypeformCreatedEvent.Agent.Id}. Templates {Enum.GetName(typeof(global::Agent.Domain.InquiryType), agentTypeformCreatedEvent.TypeFormType)}");
        }
    }
}
