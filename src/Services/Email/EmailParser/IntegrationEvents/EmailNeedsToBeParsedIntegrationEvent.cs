namespace Email.EmailParser.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public class EmailNeedsToBeParsedIntegrationEvent : IntegrationEvent
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailNeedsToBeParsedIntegrationEvent()
        {

        }
    }
}

