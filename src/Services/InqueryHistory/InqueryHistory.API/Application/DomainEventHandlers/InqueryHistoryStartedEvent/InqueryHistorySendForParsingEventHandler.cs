namespace InqueryHistory.DomainEventHandlers.ContactCreatedEvent
{
    using InqueryHistory.Command;
    using InqueryHistory.Domain;
    using InqueryHistory.Domain.Events;
    using InqueryHistory.Services;
    using InvitationHistory.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class SendEmailToCustomerEventHandler
                        : INotificationHandler<InqueryHistoryStartedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;

        public SendEmailToCustomerEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<InqueryHistory> inqueryHistoryRepository,
            IMediator mediator)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
        }

        public async Task Handle(InqueryHistoryStartedDomainEvent @event, CancellationToken cancellationToken)
        {
            //send  inquery for parsing
            var emailNeedsToBeParsed = new EmailNeedsToBeParsedIntegrationEvent
            {
                Body = @event.InqueryHistory.Message,
                Subject = @event.InqueryHistory.Subject,
                //To = new[] { "" }, //decide mailbox to be sent for parsing
                AggregateId = @event.InqueryHistory.Id
            };

            eventBus.Publish(emailNeedsToBeParsed);

            var updateInqueryStatusToSentForParsingCommand = new UpdateInqueryStatusToSentForParsingCommand()
            {
                AggregateId = @event.InqueryHistory.Id
            };

            await mediator.Send(updateInqueryStatusToSentForParsingCommand);

            logger.CreateLogger(nameof(@event)).LogTrace($"Inquery history send for parsing {@event.InqueryHistory.Id}.");
        }
    }
}
