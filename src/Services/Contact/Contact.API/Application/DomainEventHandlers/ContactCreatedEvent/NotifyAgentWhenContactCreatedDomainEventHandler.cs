namespace Contact.DomainEventHandlers.ContactCreatedEvent
{
    using Contact.Domain;
    using Contact.Domain.Events;
    using Contact.Services;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class NotifyAgentWhenContactCreatedDomainEventHandler
                        : INotificationHandler<ContactCreatedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IRepository<Contact> contactRepository;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;

        public NotifyAgentWhenContactCreatedDomainEventHandler(
            ILoggerFactory logger,
            IRepository<Contact> contactRepository,
            IIdentityService identityService,
            IEventBus eventBus)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.contactRepository = contactRepository ?? throw new ArgumentNullException(nameof(contactRepository));
        }

        public async Task Handle(ContactCreatedDomainEvent contactCreatedDomainEvent, CancellationToken cancellationToken)
        {
           logger.CreateLogger(nameof(NotifyAgentWhenContactCreatedDomainEventHandler)).LogTrace($"Agent xxx {contactCreatedDomainEvent.Email}.");
        }
    }
}
