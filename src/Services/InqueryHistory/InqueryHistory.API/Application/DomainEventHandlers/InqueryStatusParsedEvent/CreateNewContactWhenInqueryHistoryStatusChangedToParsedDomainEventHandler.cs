namespace InqueryHistory.DomainEventHandlers.ContactCreatedEvent
{
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
    using System.Linq;
    using InqueryHistory.Command;

    public class CreateNewContactWhenInqueryHistoryStatusChangedToParsedDomainEventHandler
                        : INotificationHandler<InqueryHistoryStatusChangedToParsedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;

        public CreateNewContactWhenInqueryHistoryStatusChangedToParsedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<InqueryHistory> inqueryHistoryRepository,
            IQueryExecutor queryExecutor,
            IMediator mediator)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
        }

        public async Task Handle(InqueryHistoryStatusChangedToParsedDomainEvent @event, CancellationToken cancellationToken)
        {
            //Send a new lead added email
            //var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            //{
            //    //Body = mailBody,
            //    IsBodyHtml = true,
            //    //Subject = subject,
            //    FromEmail = "admin@adfenixleads.com",
            //    FromName = "Admin",
            //    To = new[] { @event.InqueryHistory.AgentEmail },
            //    ReplyTo = "admin@adfenixleads.com",
            //    AggregateId = @event.InqueryHistory.Id,
            //    TemplateId = "954b9208-176d-44e8-af2a-8bed61e88631", //keep it hardcoded for now
            //    MergeFields = GetMergeField(@event.InqueryHistory.AgentInfo, @event)
            //};

            //eventBus.Publish(emailNeedsToBeSent);

            var createContactIntegrationEvent = new CreateContactIntegrationEvent()
            {
                AggregateId = @event.InqueryHistory.Id,
                Source = "InqueryRequest",
                Email = @event.InqueryHistory.OrganizationEmail,
                OwnerId = @event.InqueryHistory.AgentInfo.Id,
                Ownername = $"{@event.InqueryHistory.AgentInfo.Firstname} {@event.InqueryHistory.AgentInfo.Lastname}"
            };

            eventBus.Publish(createContactIntegrationEvent);

            logger.CreateLogger(nameof(@event)).LogTrace($"Inquery history email converted to leads {@event.InqueryHistory.Id} - {@event.InqueryHistory.OrganizationEmail}.");
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
                    { "[Customer_Email]", @event.InqueryHistory.OrganizationEmail },
                };

            foreach(var item in @event.InqueryHistory.ExtractedFields)
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
