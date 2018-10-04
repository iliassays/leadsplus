namespace InqueryHistory.DomainEventHandlers.ContactCreatedEvent
{
    using HtmlAgilityPack;
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
        private readonly Dictionary<string, string> buyInquiryParsingEmailMaping;
        private readonly Dictionary<string, string> rentInquiryParsingEmailMaping;

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

            //TODO: move it ot database and add dynamically
            buyInquiryParsingEmailMaping = new Dictionary<string, string>()
            {
                {"gmail.com", "adfenixleads0buyinquiry@robot.zapier.com" },
                {"rightmove.co.uk", "rightmove0co0uk0customerinquery@robot.zapier.com" },
                {"domain.com.au", "domain0com0au@robot.zapier.com" },
                {"realestate.com.au", "realestate0com0au@robot.zapier.com" }
            };

            rentInquiryParsingEmailMaping = new Dictionary<string, string>()
            {
                {"gmail.com", "adfenixleads0rentinquiry@robot.zapier.com" },
                {"rightmove.co.uk", "rightmove0co0uk0customerinquery@robot.zapier.com" },
                {"domain.com.au", "domain0com0au@robot.zapier.com" },
                {"realestate.com.au", "realestate0com0au@robot.zapier.com" }
            };
        }

        public async Task Handle(InqueryHistoryStartedDomainEvent @event, CancellationToken cancellationToken)
        {
            var organizationDomain = @event.InqueryHistory.OrganizationInfo.OrganizationEmail.Split("@")[1];

            var parserEmail = buyInquiryParsingEmailMaping.ContainsKey(organizationDomain) ? buyInquiryParsingEmailMaping[organizationDomain] : "adfenixleads0buyinquiry@robot.zapier.com";

            if (@event.InqueryHistory.InquiryType == InquiryType.RentInquiry)
            {
                parserEmail = rentInquiryParsingEmailMaping.ContainsKey(organizationDomain) ? rentInquiryParsingEmailMaping[organizationDomain] : "adfenixleads0rentinquiry@robot.zapier.com";
            }

            logger.CreateLogger(nameof(@event)).LogTrace($"Inquery parser email {parserEmail}.");

            //send  inquery for parsing
            var emailNeedsToBeParsed = new EmailNeedsToBeParsedIntegrationEvent
            {
                Body = @event.InqueryHistory.MessagePlainText,
                Subject = @event.InqueryHistory.Subject,
                ToEmail = parserEmail, 
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

        public string FormatMessage(string message)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(message);

            var aTags = doc.DocumentNode.SelectNodes("//a");

            if (aTags != null)
            {
                //iterate through all the anchor tags and retrieve its href attribute value(link)
                foreach (var aTag in aTags)
                {
                    aTag.Attributes.Add("clicktracking", "off");
                    aTag.Attributes.Add("opentracking", "off");
                }
            }

            return doc.DocumentNode.InnerHtml;
        }
    }
}
