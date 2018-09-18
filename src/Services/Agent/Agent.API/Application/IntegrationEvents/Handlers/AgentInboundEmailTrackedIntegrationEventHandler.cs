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

    public class AgentInboundEmailTrackedIntegrationEventHandler
        : IIntegrationEventHandler<AgentInboundEmailTrackedIntegrationEvent>
    {
        private readonly IEventBus eventBus;
        private readonly IQueryExecutor queryExecutor;

        public AgentInboundEmailTrackedIntegrationEventHandler(IEventBus eventBus,
            IQueryExecutor queryExecutor)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.queryExecutor = queryExecutor ?? throw new ArgumentNullException(nameof(queryExecutor));
        }

        public async Task Handle(AgentInboundEmailTrackedIntegrationEvent @event)
        {
            var agent = await queryExecutor.Execute<GetAgentByIntegrationEmailQuery, Agent>(
                new GetAgentByIntegrationEmailQuery {AgentIntegrationEmail = @event.AgentEmail});

            WelcomeCustomer(agent, @event);
            NotifyAgent(agent, @event);

            CreateCustomer(agent, @event);
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
                TemplateId = "ed324a45-f3a7-4232-a551-12abc8051798", //keep it hardcoded for now
                MergeFields = GetMergeField(agent, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);
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
                TemplateId = "954b9208-176d-44e8-af2a-8bed61e88631", //keep it hardcoded for now
                MergeFields = GetMergeField(agent, @event)
            };

            eventBus.Publish(emailNeedsToBeSent);
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
