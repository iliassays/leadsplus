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
    using LeadsPlus.GoogleApis.Command;

    public class UpdateSpreadsheetWhenInqueryHistoryStatusChangedToParsedDomainEventHandler
                        : INotificationHandler<InqueryHistoryStatusChangedToParsedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;

        public UpdateSpreadsheetWhenInqueryHistoryStatusChangedToParsedDomainEventHandler(
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
            InsertRowToSpreadsheetCommand insertRowToSpreadsheetCommand = new InsertRowToSpreadsheetCommand
            {
                SpreadSheetId = @event.InqueryHistory.AgentInfo.AgentInquiryInfo.SpreadsheetId,
                WorkSheetName = Enum.GetName(typeof(InquiryType), @event.InqueryHistory.InquiryType),
                ApplicationName = "LeadsPlus",
                Values = new List<object>()
                {
                    @event.InqueryHistory.Id,
                    @event.InqueryHistory.CreatedDate,
                    $"{@event.InqueryHistory.CustomerInfo.Firstname} {@event.InqueryHistory.CustomerInfo.Lastname}",
                    @event.InqueryHistory.CustomerInfo.Email,
                    @event.InqueryHistory.CustomerInfo.Phone,
                    @event.InqueryHistory.CustomerInfo.Address,
                    @event.InqueryHistory.CustomerInfo.Aboutme,                    
                    @event.InqueryHistory.OrganizationInfo.OrganizationDomain,
                    Enum.GetName(typeof(InquiryType), @event.InqueryHistory.InquiryType),
                    @event.InqueryHistory.PropertyInfo.Message,
                    @event.InqueryHistory.PropertyInfo.PropertyAddress,
                    @event.InqueryHistory.PropertyInfo.ReferenceNo,
                    @event.InqueryHistory.PropertyInfo.PropertyUrl,
                }
            };

            var spreadsheet = mediator.Send(insertRowToSpreadsheetCommand).Result;


            logger.CreateLogger(nameof(@event)).LogTrace($"Inquery history spreadsheet updated. Inquiry history: {@event.InqueryHistory.Id} - Inquiry from: {@event.InqueryHistory.OrganizationInfo.OrganizationEmail}.");
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
                    { "[addressbooklink]", "http://contact.adfenixleads.com" },
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
