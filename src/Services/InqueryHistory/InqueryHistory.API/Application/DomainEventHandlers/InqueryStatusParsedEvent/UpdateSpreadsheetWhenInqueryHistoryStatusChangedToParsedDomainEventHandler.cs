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
                SpreadSheetId = @event.InqueryHistory.AgentInquiryInfo.SpreadsheetId,
                WorkSheetName = Enum.GetName(typeof(InquiryType), @event.InqueryHistory.InquiryType),
                ApplicationName = "LeadsPlus",
                Values = new List<object>()
                {
                    @event.InqueryHistory.Id,
                    @event.InqueryHistory.CreatedDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
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

            await CreateAggregateSpreadsheetRow(@event);

            logger.CreateLogger(nameof(@event)).LogTrace($"Inquery history spreadsheet updated. Inquiry history: {@event.InqueryHistory.Id} - Inquiry from: {@event.InqueryHistory.OrganizationInfo.OrganizationEmail}.");
        }

        private async Task<bool> CreateAggregateSpreadsheetRow(InqueryHistoryStatusChangedToParsedDomainEvent @event)
        {
            logger.CreateLogger(nameof(@event)).LogTrace($"Start: Inquery history aggregate spreadsheet updated. Inquiry history: {@event.InqueryHistory.Id} - spreadsheet: {@event.InqueryHistory.AgentInquiryInfo.AggregateShareableUrl}.");

            try
            {


                InsertRowToSpreadsheetCommand insertRowToSpreadsheetCommand = new InsertRowToSpreadsheetCommand
                {
                    SpreadSheetId = @event.InqueryHistory.AgentInquiryInfo.AggregateShareableUrl,
                    WorkSheetName = "Aggregate",
                    ApplicationName = "LeadsPlus",
                    Values = new List<object>()
                {
                    @event.InqueryHistory.Id,
                    @event.InqueryHistory.CreatedDate.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                    Enum.GetName(typeof(InquiryType), @event.InqueryHistory.InquiryType),
                    "FALSE",
                    "FALSE",
                    "FALSE",
                    "FALSE"
                }
                };

                var spreadsheet = mediator.Send(insertRowToSpreadsheetCommand).Result;
            }
            catch(Exception ex)
            {
                logger.CreateLogger(nameof(@event)).LogTrace(ex.StackTrace);
            }

            logger.CreateLogger(nameof(@event)).LogTrace($"End: Inquery history aggregate spreadsheet updated. Inquiry history: {@event.InqueryHistory.Id} - spreadsheet: {@event.InqueryHistory.AgentInquiryInfo.AggregateShareableUrl}.");

            return true;
        }
    }
}
