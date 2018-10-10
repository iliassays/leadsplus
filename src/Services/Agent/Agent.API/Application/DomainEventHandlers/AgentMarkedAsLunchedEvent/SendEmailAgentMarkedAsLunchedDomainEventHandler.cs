namespace Agent.DomainEventHandlers
{
    using Agent.Domain.Events;
    using Agent.IntegrationEvents;
    using Agent.Services;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using LeadsPlus.Core.Query;
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class SendEmailAgentMarkedAsLunchedDomainEventHandler
                        : INotificationHandler<AgentMarkedAsLaunchedEvent>
    {
        private readonly ILoggerFactory logger;
        private readonly IIdentityService identityService;
        private readonly IEventBus eventBus;
        private readonly IQueryExecutor queryExecutor;
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public SendEmailAgentMarkedAsLunchedDomainEventHandler(
            ILoggerFactory logger,
            IIdentityService identityService,
            IEventBus eventBus,
            IQueryExecutor queryExecutor,
            IMediator mediator,
            IConfiguration configuration)
        {
            this.identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task Handle(AgentMarkedAsLaunchedEvent @event, CancellationToken cancellationToken)
        {
            var agentLunchedNotificationTemplate = configuration.GetValue<string>("AgentLunchedNotificationTemplate");

            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                //Body = mailBody,
                IsBodyHtml = true,
                //Subject = subject,
                FromEmail = "admin@adfenixleads.com",
                FromName = "AdfenixLeads",
                To = new[] { @event.Agent.Email },
                ReplyTo = "admin@adfenixleads.com",
                AggregateId = @event.Agent.Id,
                TemplateId = agentLunchedNotificationTemplate,
                MergeFields = GetMergeField(@event.Agent)
            };

            eventBus.Publish(emailNeedsToBeSent);

            logger.CreateLogger(nameof(@event)).LogTrace($"Agent lunched notification sent {@event.Agent.Id} - {@event.Agent.Email} -templateid {emailNeedsToBeSent.TemplateId}.");
        }

        private Dictionary<string, string> GetMergeField(Domain.Agent agent)
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
                    { "[agentbuyinquirytypeformlink]", agent.BuyInquiry.TypeFormUrl },
                    { "[agentbuyinquiryspreadsheetlink]", agent.BuyInquiry.SpreadsheetShareableUrl },
                    { "[agentmortgagespreadsheetlink]", agent.BuyInquiry.MortgageSpreadsheetShareableUrl },
                    { "[agentrentinquirytypeformlink]", agent.RentInquiry.TypeFormUrl },
                    { "[agentrentinquiryspreadsheetlink]", agent.RentInquiry.SpreadsheetShareableUrl },
                    { "[agentlandlordspreadsheetlink]", agent.RentInquiry.LandlordSpreadsheetShareableUrl }
                };

            return mergedFields;
        }
    }
}
