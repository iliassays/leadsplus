namespace InqueryHistory.DomainEventHandlers.ContactCreatedEvent
{
    using InqueryHistory.Command;
    using InqueryHistory.Domain;
    using InqueryHistory.Domain.Events;
    using InqueryHistory.Services;
    using InvitationHistory.IntegrationEvents;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Query;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class SendEmailToCustomerWhenInqueryHistoryStatusChangedToParsedDomainEventHandler
                        : INotificationHandler<InqueryHistoryStatusChangedToParsedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;

        public SendEmailToCustomerWhenInqueryHistoryStatusChangedToParsedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<InqueryHistory> inqueryHistoryRepository,
            IQueryExecutor queryExecutor,
            IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
        }

        public async Task Handle(InqueryHistoryStatusChangedToParsedDomainEvent @event, CancellationToken cancellationToken)
        {
            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                //Body = mailBody,
                IsBodyHtml = true,
                //Subject = subject,
                FromEmail = @event.InqueryHistory.AgentEmail,
                FromName = $"{@event.InqueryHistory.AgentInfo.Firstname} {@event.InqueryHistory.AgentInfo.Lastname}",
                To = new[] { @event.InqueryHistory.CustomerEmail },
                ReplyTo = @event.InqueryHistory.AgentEmail,
                AggregateId = @event.InqueryHistory.Id,
                TemplateId = "ed324a45-f3a7-4232-a551-12abc8051798", //keep it hardcoded for now
                MergeFields = GetMergeField(@event.InqueryHistory.AgentInfo, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);

            var updateCustomerAutoresponderSentCommand = new UpdateCustomerAutoresponderSentCommand()
            {
                AggregateId = @event.InqueryHistory.Id
            };

            await mediator.Send(updateCustomerAutoresponderSentCommand);

            logger.CreateLogger(nameof(@event)).LogTrace($"Autoresponder sent to customer {@event.InqueryHistory.CustomerEmail}-{@event.InqueryHistory.CustomerEmail}.");
        }

        private Dictionary<string, string> GetMergeField(Domain.AgentInfo agent, InqueryHistoryStatusChangedToParsedDomainEvent @event)
        {
            var mergedFields = new Dictionary<string, string>()
                {
                    { "[Sender_Name]", $"{agent.Firstname} {agent.Lastname}" },
                    { "[Sender_Address]", agent.Address },
                    { "[Sender_City]", agent.City },
                    { "[Sender_State]", agent.State },
                    { "[Sender_Zip]", agent.Zip },
                    { "[Typeform_Link]", agent.AgentTypeFormInfo.TypeFormUrl },
                    { "[Lead_Link]", "http://contact.adfenixleads.com" },
                    { "[Lead_Spreadsheet]", agent.AgentTypeFormInfo.SpreadsheetUrl },
                    { "[Customer_Email]", @event.InqueryHistory.CustomerEmail },
                };

            foreach (var item in @event.InqueryHistory.ExtractedFields)
            {
                if (!mergedFields.ContainsKey(item.Key))
                {
                    mergedFields.Add(item.Key, item.Value);
                }
            }

            return mergedFields;
        }
    }
}
