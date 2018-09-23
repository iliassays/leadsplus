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
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class InqueryHistorySendForParsingEventHandler
                        : INotificationHandler<InqueryHistoryStartedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IMediator mediator;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly Dictionary<string, string> parsingEmailMaping;

        public InqueryHistorySendForParsingEventHandler(
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

            parsingEmailMaping = new Dictionary<string, string>()
            {
                {"gmail.com", "adfenixleads@robot.zapier.com" },
                {"rightmove.co.uk", "rightmove0co0uk0customerinquery@robot.zapier.com" },
                {"domain.com.au", "domain0com0au@robot.zapier.com" }
            };
        }

        public async Task Handle(InqueryHistoryStartedDomainEvent @event, CancellationToken cancellationToken)
        {
            var organizationDomain = @event.InqueryHistory.OrganizationEmail.Split("@")[1];
            //send  inquery for parsing
            var emailNeedsToBeParsed = new EmailNeedsToBeParsedIntegrationEvent
            {
                Body = @event.InqueryHistory.Message,
                Subject = @event.InqueryHistory.Subject,
                ToEmail = parsingEmailMaping.ContainsKey(organizationDomain) ? parsingEmailMaping[organizationDomain] : "adfenixleads@robot.zapier.com", 
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
