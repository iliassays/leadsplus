namespace Agent.IntegrationEvents
{
    using Agent.Domain;
    using Agent.Domain.Query;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using System;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;
    using System.Collections.Generic;
    using System.Collections;
    using Microsoft.Extensions.Logging;

    public class AgentInboundEmailTrackedIntegrationEventHandler
        : IIntegrationEventHandler<AgentInboundEmailTrackedIntegrationEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IQueryExecutor queryExecutor;
        private readonly ILoggerFactory logger;

        public AgentInboundEmailTrackedIntegrationEventHandler(IEventBus eventBus,
            IQueryExecutor queryExecutor,
            ILoggerFactory logger)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(AgentInboundEmailTrackedIntegrationEvent @event)
        {
            logger.CreateLogger(nameof(@event)).LogTrace($"customer email tracked {@event.OrganizationEmail}.");
            logger.CreateLogger(nameof(@event)).LogTrace($"agent email {@event.AgentEmail}.");

            var agent = await queryExecutor.Execute<GetAgentByIntegrationEmailQuery, Agent>(
                new GetAgentByIntegrationEmailQuery {AgentIntegrationEmail = @event.AgentEmail});

            if(agent != null)
            {
                logger.CreateLogger(nameof(@event)).LogTrace($"agent email {agent.Id}.");

                NewInqueryRequestReceivedIntegrationEvent newInqueryRequestReceivedIntegrationEvent = new NewInqueryRequestReceivedIntegrationEvent()
                {
                    AgentEmail = @event.AgentEmail,
                    AggregateId = @event.AggregateId,
                    Body = @event.Body,
                    Subject = @event.Subject,
                    OrganizationEmail = @event.OrganizationEmail,
                    InquiryType = (int)GetInquiryType(@event),
                    PlainText = @event.PlainText,

                    AgentInfo = new AgentInfo()
                    {
                        Address = agent.Address,
                        City = agent.City,
                        State = agent.State,
                        Zip = agent.Zip,
                        Email = agent.Email,
                        Firstname = agent.Firstname,
                        Lastname = agent.Lastname,
                        Company = agent.Company,
                        Country = agent.Country,
                        Phone = agent.Phone,
                        Logo = agent.Logo,
                        Id = agent.Id,
                        IntegrationEmail = agent.IntegrationEmail,
                        Facebook = agent.Facebook,
                        Instagram = agent.Instagram,
                        LinkedIn = agent.LinkedIn,
                        Twitter = agent.Twitter
                    },
                    AgentInquiryInfo = GetInquiryInfoData(@event, agent),
                    AgentAutoresponderTemplateInfo = GetAutoresponderTemplateData(@event, agent)
                };

                eventBus.Publish(newInqueryRequestReceivedIntegrationEvent);
            }
            else
            {
                logger.CreateLogger(nameof(@event)).LogTrace($"agent not found {@event.AgentEmail}.");
            }
        }

        private AgentInquiryInfo GetInquiryInfoData(AgentInboundEmailTrackedIntegrationEvent @event, Agent agent)
        {
            if (GetInquiryType(@event) == InquiryType.RentInquiry) 
            {
                return new AgentInquiryInfo
                {
                    SpreadsheetId = agent.RentInquiry?.SpreadsheetId,
                    SpreadsheetName = agent.RentInquiry?.SpreadsheetName,
                    SpreadsheetUrl = agent.RentInquiry?.SpreadsheetUrl,
                    TypeFormUrl = agent.RentInquiry?.TypeFormUrl,
                    SpreadsheetShareableUrl = agent.RentInquiry?.SpreadsheetShareableUrl,
                    LandlordShareableUrl = agent.RentInquiry?.LandlordSpreadsheetShareableUrl,
                };
            }
            else
            {
                return new AgentInquiryInfo
                {
                    SpreadsheetId = agent.BuyInquiry?.SpreadsheetId,
                    SpreadsheetName = agent.BuyInquiry?.SpreadsheetName,
                    SpreadsheetUrl = agent.BuyInquiry?.SpreadsheetUrl,
                    TypeFormUrl = agent.BuyInquiry?.TypeFormUrl,
                    SpreadsheetShareableUrl = agent.BuyInquiry?.SpreadsheetShareableUrl,
                    MortgageShareableUrl = agent.BuyInquiry?.MortgageSpreadsheetShareableUrl
                };
            }
        }

        private AgentAutoresponderTemplateInfo GetAutoresponderTemplateData(AgentInboundEmailTrackedIntegrationEvent @event, Agent agent)
        {
            if (GetInquiryType(@event) == InquiryType.RentInquiry)
            {
                return new AgentAutoresponderTemplateInfo
                {
                    CustomerAutoresponderTemplateId = agent.RentInquiry?.InquiryAutoresponderTemplate?.CustomerAutoresponderTemplateId,
                    AgentAutoresponderTemplateId = agent.RentInquiry?.InquiryAutoresponderTemplate?.AgentAutoresponderTemplateId
                };
            }
            else
            {
                return new AgentAutoresponderTemplateInfo
                {
                    CustomerAutoresponderTemplateId = agent.BuyInquiry?.InquiryAutoresponderTemplate?.CustomerAutoresponderTemplateId,
                    AgentAutoresponderTemplateId = agent.BuyInquiry?.InquiryAutoresponderTemplate?.AgentAutoresponderTemplateId
                };
            }
        }

        public InquiryType GetInquiryType(AgentInboundEmailTrackedIntegrationEvent @event)
        {
            if (@event.Subject.Contains("Rent", StringComparison.InvariantCultureIgnoreCase)
                || @event.PlainText.Contains("Rent", StringComparison.InvariantCultureIgnoreCase) 
                || @event.PlainText.Contains("Tenant", StringComparison.InvariantCultureIgnoreCase) 
                || @event.PlainText.Contains("Tenant", StringComparison.InvariantCultureIgnoreCase))//Bad way. Need to fugure out something after seeing some templates
            {
                return InquiryType.RentInquiry;
            }

            return InquiryType.BuyInquiry;
        }
    }
}
