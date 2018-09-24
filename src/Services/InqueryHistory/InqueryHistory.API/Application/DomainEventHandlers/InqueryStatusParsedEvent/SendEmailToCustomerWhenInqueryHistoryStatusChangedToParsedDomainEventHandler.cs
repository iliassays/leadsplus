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
                ReplyTo = @event.InqueryHistory.AgentInfo.Email,
                AggregateId = @event.InqueryHistory.Id,
                TemplateId = "bf191e71-8916-424f-a2ad-3c15a058ac22", //Autoresponder for new Customer. keep it hardcoded for now
                MergeFields = GetMergeField(@event.InqueryHistory.AgentInfo, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);

            var updateCustomerAutoresponderSentCommand = new UpdateCustomerAutoresponderSentCommand()
            {
                AggregateId = @event.InqueryHistory.Id
            };

            await mediator.Send(updateCustomerAutoresponderSentCommand);

            logger.CreateLogger(nameof(@event)).LogTrace($"Autoresponder sent to customer {@event.InqueryHistory.OrganizationEmail}-{@event.InqueryHistory.OrganizationEmail}.");
        }

        private Dictionary<string, string> GetMergeField(Domain.AgentInfo agent, InqueryHistoryStatusChangedToParsedDomainEvent @event)
        {
            var mergedFields = new Dictionary<string, string>()
                {
                    { "[agentfirstname]", agent.Firstname },
                    { "[agentlastname]", agent.Lastname },
                    { "[agentaddress]", agent.Address },
                    { "[agentcity]", agent.City },
                    { "[agentstate]", agent.State },
                    { "[agentzip]", agent.Zip },
                    { "[agenttypeformlink]", agent.AgentTypeFormInfo.TypeFormUrl },
                    { "[addressbooklink]", "http://contact.adfenixleads.com" },
                    { "[inquirylist]", agent.AgentTypeFormInfo.SpreadsheetUrl },
                    { "[organizationemail]", @event.InqueryHistory.OrganizationEmail },
                };

            foreach (var item in @event.InqueryHistory.ExtractedFields)
            {
                if (!mergedFields.ContainsKey(item.Key))
                {
                    mergedFields.Add($"[{item.Key}]", item.Value);
                }
            }

            return mergedFields;
        }
    }
}
