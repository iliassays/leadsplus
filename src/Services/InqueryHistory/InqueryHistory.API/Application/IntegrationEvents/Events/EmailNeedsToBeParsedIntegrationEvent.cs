namespace InvitationHistory.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public class EmailNeedsToBeParsedIntegrationEvent : IntegrationEvent
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailNeedsToBeParsedIntegrationEvent()
        {

        }
    }
}

