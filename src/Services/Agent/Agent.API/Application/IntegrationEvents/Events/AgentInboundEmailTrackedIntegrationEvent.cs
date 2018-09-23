namespace Agent.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;

    public class AgentInboundEmailTrackedIntegrationEvent : IntegrationEvent
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string OrganizationEmail { get; set; }
        public string AgentEmail { get; set; }
        public string PlainText { get; set; }

        public AgentInboundEmailTrackedIntegrationEvent()
        {
            
        }
    }
}
