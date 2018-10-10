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
    using Microsoft.Extensions.Configuration;

    public class SendEmailToAgentWhenInqueryHistoryStatusChangedToParsedDomainEventHandler
                        : INotificationHandler<InqueryHistoryStatusChangedToParsedDomainEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IRepository<InqueryHistory> inqueryHistoryRepository;
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public SendEmailToAgentWhenInqueryHistoryStatusChangedToParsedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IRepository<InqueryHistory> inqueryHistoryRepository,
            IQueryExecutor queryExecutor,
            IMediator mediator,
            IConfiguration configuration)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.inqueryHistoryRepository = inqueryHistoryRepository ?? throw new ArgumentNullException(nameof(inqueryHistoryRepository));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Handle(InqueryHistoryStatusChangedToParsedDomainEvent @event, CancellationToken cancellationToken)
        {
            //msut create some template factory if more inquiry type comes. This is bad approach buy quicker for now
            var agentAutoresponderDefaultTemplateId = configuration.GetValue<string>("AgentAutoresponderDefaultTemplateIdForBuyInquiry");

            if (@event.InqueryHistory.InquiryType == InquiryType.RentInquiry)
            {
                agentAutoresponderDefaultTemplateId = configuration.GetValue<string>("AgentAutoresponderDefaultTemplateIdForRentInquiry");
            }

            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                //Body = mailBody,
                IsBodyHtml = true,
                //Subject = subject,
                FromEmail = "admin@adfenixleads.com",
                FromName = "AdfenixLeads",
                To = new[] { @event.InqueryHistory.AgentInfo.Email },
                ReplyTo = @event.InqueryHistory.CustomerInfo.Email,
                AggregateId = @event.InqueryHistory.Id,
                TemplateId =  string.IsNullOrEmpty(@event.InqueryHistory.AgentAutoresponderTemplateInfo?.AgentAutoresponderTemplateId) ? 
                                                    agentAutoresponderDefaultTemplateId :
                                                    @event.InqueryHistory.AgentAutoresponderTemplateInfo?.AgentAutoresponderTemplateId, //Autoresponder for agent For new Inquiry. keep it hardcoded for now
                MergeFields = GetMergeField(@event.InqueryHistory.AgentInfo, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);

            var updateAgentAutoresponderSentCommand = new UpdateAgentAutoresponderSentCommand()
            {
                AggregateId = @event.InqueryHistory.Id
            };

            await mediator.Send(updateAgentAutoresponderSentCommand);

            logger.CreateLogger(nameof(@event)).LogTrace($"Inquery history email send to agent {@event.InqueryHistory.Id} - {@event.InqueryHistory.AgentInfo.Email} -templateid {emailNeedsToBeSent.TemplateId}.");
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
                    { "[agentfacebook]", agent.Facebook },
                    { "[agentinstagram]", agent.Instagram },
                    { "[agenttwitter]", agent.Twitter },
                    { "[agentlinkedIn]", agent.LinkedIn },
                    { "[agentlogo]", agent.Logo },
                    { "[agentinquirytypeformlink]", @event.InqueryHistory.GenerateTypeFormLink(@event.InqueryHistory.AgentInquiryInfo.TypeFormUrl) },
                    { "[addressbooklink]", "http://contact.adfenixleads.com" },
                    { "[agentinquiryspreadsheetlink]", @event.InqueryHistory.AgentInquiryInfo?.SpreadsheetUrl },
                    { "[agentmortgagespreadsheetlink]", @event.InqueryHistory.AgentInquiryInfo?.MortgageShareableUrl },
                    { "[agentlandlordspreadsheetlink]", @event.InqueryHistory.AgentInquiryInfo?.LandlordShareableUrl },
                    { "[organizationemail]", @event.InqueryHistory.OrganizationInfo.OrganizationEmail },
                    { "[organizationdomain]", @event.InqueryHistory.OrganizationInfo.OrganizationDomain },
                    { "[organizationname]", @event.InqueryHistory.OrganizationInfo.OrganizationName }
                };

            foreach(var item in @event.InqueryHistory.ExtractedFields)
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
