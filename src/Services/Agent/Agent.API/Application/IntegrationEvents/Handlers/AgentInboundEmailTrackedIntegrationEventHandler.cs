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
                    InquiryType = GetInquiryType(@event),
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
                        Id = agent.Id,
                        IntegrationEmail = agent.IntegrationEmail,
                        InquiryTypeForm = GetTypeformData(@event, agent),
                        AgentAutoresponderTemplateInfo = GetAutoresponderTemplateData(@event, agent),
                    }
                };

                eventBus.Publish(newInqueryRequestReceivedIntegrationEvent);
            }
            else
            {
                logger.CreateLogger(nameof(@event)).LogTrace($"agent not found {@event.AgentEmail}.");
            }
        }

        private AgentTypeFormInfo GetTypeformData(AgentInboundEmailTrackedIntegrationEvent @event, Agent agent)
        {
            if (GetInquiryType(@event) == InquiryType.RentInquiry) 
            {
                return new AgentTypeFormInfo
                {
                    SpreadsheetId = agent.RentInquiryTypeForm?.SpreadsheetId,
                    SpreadsheetName = agent.RentInquiryTypeForm?.SpreadsheetName,
                    SpreadsheetUrl = agent.RentInquiryTypeForm?.SpreadsheetUrl,
                    TypeFormUrl = agent.RentInquiryTypeForm?.TypeFormUrl
                };
            }
            else
            {
                return new AgentTypeFormInfo
                {
                    SpreadsheetId = agent.BuyInquiryTypeForm?.SpreadsheetId,
                    SpreadsheetName = agent.BuyInquiryTypeForm?.SpreadsheetName,
                    SpreadsheetUrl = agent.BuyInquiryTypeForm?.SpreadsheetUrl,
                    TypeFormUrl = agent.BuyInquiryTypeForm?.TypeFormUrl
                };
            }
        }

        private AgentAutoresponderTemplateInfo GetAutoresponderTemplateData(AgentInboundEmailTrackedIntegrationEvent @event, Agent agent)
        {
            if (GetInquiryType(@event) == InquiryType.RentInquiry)
            {
                return new AgentAutoresponderTemplateInfo
                {
                    CustomerAutoresponderTemplateId = agent.RentInquiryAutoresponderTemplate?.CustomerAutoresponderTemplateId,
                    AgentAutoresponderTemplateId = agent.RentInquiryAutoresponderTemplate?.AgentAutoresponderTemplateId
                };
            }
            else
            {
                return new AgentAutoresponderTemplateInfo
                {
                    CustomerAutoresponderTemplateId = agent.BuyInquiryAutoresponderTemplate?.CustomerAutoresponderTemplateId,
                    AgentAutoresponderTemplateId = agent.BuyInquiryAutoresponderTemplate?.AgentAutoresponderTemplateId
                };
            }
        }

        public InquiryType GetInquiryType(AgentInboundEmailTrackedIntegrationEvent @event)
        {
            if (@event.Subject.Contains("Rent") || @event.PlainText.Contains("Rent"))//Bad way. Need to fugure out something after seeing some templates
            {
                return InquiryType.RentInquiry;
            }

            return InquiryType.BuyInquiry;
        }
    }
}
