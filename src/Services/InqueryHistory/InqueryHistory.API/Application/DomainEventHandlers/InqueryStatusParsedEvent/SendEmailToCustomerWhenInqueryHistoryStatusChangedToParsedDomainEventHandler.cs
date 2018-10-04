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
    using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public SendEmailToCustomerWhenInqueryHistoryStatusChangedToParsedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<InqueryHistory> inqueryHistoryRepository,
            IQueryExecutor queryExecutor,
            IMediator mediator,
            IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Handle(InqueryHistoryStatusChangedToParsedDomainEvent @event, CancellationToken cancellationToken)
        {
            //msut create some template factory if more inquiry type comes. This is bad approach buy quicker for now
            var customerAutoresponderDefaultTemplateId = configuration.GetValue<string>("CustomerAutoresponderDefaultTemplateIdForBuyInquiry");

            if(@event.InqueryHistory.InquiryType == InquiryType.RentInquiry)
            {
                customerAutoresponderDefaultTemplateId = configuration.GetValue<string>("CustomerAutoresponderDefaultTemplateIdForRentInquiry");
            }

            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                //Body = mailBody,
                IsBodyHtml = true,
                //Subject = subject,
                FromEmail = @event.InqueryHistory.AgentEmail,
                FromName = $"{@event.InqueryHistory.AgentInfo.Firstname} {@event.InqueryHistory.AgentInfo.Lastname}",
                To = new[] { @event.InqueryHistory.CustomerInfo.Email },
                ReplyTo = @event.InqueryHistory.AgentInfo.Email,
                AggregateId = @event.InqueryHistory.Id,
                TemplateId = string.IsNullOrEmpty(@event.InqueryHistory.AgentAutoresponderTemplateInfo?.CustomerAutoresponderTemplateId) ?
                                                    customerAutoresponderDefaultTemplateId :
                                                    @event.InqueryHistory.AgentAutoresponderTemplateInfo?.CustomerAutoresponderTemplateId, //Autoresponder for agent For new Inquiry. keep it hardcoded for now
                MergeFields = GetMergeField(@event.InqueryHistory.AgentInfo, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);

            var updateCustomerAutoresponderSentCommand = new UpdateCustomerAutoresponderSentCommand()
            {
                AggregateId = @event.InqueryHistory.Id
            };

            await mediator.Send(updateCustomerAutoresponderSentCommand);

            logger.CreateLogger(nameof(@event)).LogTrace($"Autoresponder sent to customer {@event.InqueryHistory.CustomerInfo.Email}- inquiryId: {@event.InqueryHistory.Id}.");
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
                    { "[agentinquirytypeformlink]", @event.InqueryHistory.GenerateTypeFormLink(@event.InqueryHistory.AgentInquiryInfo.TypeFormUrl) },
                    { "[addressbooklink]", "http://contact.adfenixleads.com" },
                    { "[agentinquiryspreadsheetlink]", @event.InqueryHistory.AgentInquiryInfo?.SpreadsheetUrl },
                    { "[organizationemail]", @event.InqueryHistory.OrganizationInfo.OrganizationEmail }
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
