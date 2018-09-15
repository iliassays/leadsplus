namespace Agent.IntegrationEvents
{
    using Agent.Domain;
    using Agent.Domain.Query;
    using LeadsPlus.BuildingBlocks.EventBus.Abstractions;
    using LeadsPlus.Core;
    using System;
    using System.Threading.Tasks;
    using LeadsPlus.Core.Query;

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

            var subject = $"Thanks! you for your enquiry";

            var mailBody = $"Hi,\n.Thanks!\nWhy stop here? Thanks for helping us build trust and empowering agent.)\nTell up something more about yourself {agent.AgentTypeForm.TypeFormUrl}";

            var emailNeedsToBeSent = new EmailNeedsToBeSentIntegrationEvent
            {
                Body = mailBody,
                IsBodyHtml = false,
                Subject = subject,
                To = new[] { @event.CustomerEmail },
                ReplyTo = string.Empty,
                AggregateId = @event.AggregateId
            };

            eventBus.Publish(emailNeedsToBeSent);

            var createContactIntegrationEvent = new CreateContactIntegrationEvent()
            {
                AggregateId = @event.AggregateId,
                Source = "InnoundEmail",
                Email = @event.CustomerEmail
            };

            eventBus.Publish(createContactIntegrationEvent);
        }
    }
}
