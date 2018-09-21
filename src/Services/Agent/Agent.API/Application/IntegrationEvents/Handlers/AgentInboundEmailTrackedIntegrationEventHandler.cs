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
            logger.CreateLogger(nameof(@event)).LogTrace($"customer email tracked {@event.CustomerEmail}.");
            logger.CreateLogger(nameof(@event)).LogTrace($"agent email {@event.AgentEmail}.");

            var agent = await queryExecutor.Execute<GetAgentByIntegrationEmailQuery, Agent>(
                new GetAgentByIntegrationEmailQuery {AgentIntegrationEmail = @event.AgentEmail});

            if(agent != null)
            {
                logger.CreateLogger(nameof(@event)).LogTrace($"agent email {agent.Id}.");

                WelcomeCustomer(agent, @event);
                NotifyAgent(agent, @event);

                CreateCustomer(agent, @event);

            }
            else
            {
                logger.CreateLogger(nameof(@event)).LogTrace($"agent not found {@event.AgentEmail}.");
            }
        }

        private void CreateCustomer(Agent agent, AgentInboundEmailTrackedIntegrationEvent @event)
        {
            var createContactIntegrationEvent = new CreateContactIntegrationEvent()
            {
                AggregateId = @event.AggregateId,
                Source = "CloudMailin",
                Email = @event.CustomerEmail,
                OwnerId = agent.Id,
                Ownername = $"{agent.Firstname} {agent.Lastname}"
            };

            eventBus.Publish(createContactIntegrationEvent);
            logger.CreateLogger(nameof(@event)).LogTrace($"customer created published {@event.CustomerEmail}.");
        }

        private void WelcomeCustomer(Agent agent, AgentInboundEmailTrackedIntegrationEvent @event)
        {
            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                //Body = mailBody,
                IsBodyHtml = true,
                //Subject = subject,
                FromEmail = agent.Email,
                FromName = $"{agent.Firstname} {agent.Lastname}",
                To = new[] { @event.CustomerEmail },
                ReplyTo = agent.Email,
                AggregateId = @event.AggregateId,
                TemplateId = "5b74cb32-4964-4819-b702-4db122be5762", //keep it hardcoded for now
                MergeFields = GetMergeField(agent, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);

            logger.CreateLogger(nameof(@event)).LogTrace($"customer welcome event sent {@event.CustomerEmail}.");
        }

        private void NotifyAgent(Agent agent, AgentInboundEmailTrackedIntegrationEvent @event)
        {
            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                //Body = mailBody,
                IsBodyHtml = true,
                //Subject = subject,
                FromEmail = "admin@adfenixleads.com",
                FromName = "Admin",
                To = new[] { agent.Email },
                ReplyTo = "admin@adfenixleads.com",
                AggregateId = @event.AggregateId,
                TemplateId = "7093a1bf-a252-4ff0-9465-7c1e9b0a7081", //keep it hardcoded for now
                MergeFields = GetMergeField(agent, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);
            logger.CreateLogger(nameof(@event)).LogTrace($"agent email sent {@event.AgentEmail}.");
        }

        private Dictionary<string, string> GetMergeField(Agent agent, AgentInboundEmailTrackedIntegrationEvent @event)
        {
            return new Dictionary<string, string>()
                {
                    { "[Sender_Name]", $"{agent.Firstname} {agent.Lastname}" },
                    { "[Sender_Address]", agent.Address },
                    { "[Sender_City]", agent.City },
                    { "[Sender_State]", agent.State },
                    { "[Sender_Zip]", agent.Zip },
                    { "[Typeform_Link]", agent.AgentTypeForm.TypeFormUrl },
                    { "[Lead_Link]", "http://contact.adfenixleads.com" },
                    { "[Lead_Spreadsheet]", agent.AgentTypeForm.SpreadsheetUrl },
                    { "[Customer_Email]", @event.CustomerEmail },
                };
        }
    }
}
