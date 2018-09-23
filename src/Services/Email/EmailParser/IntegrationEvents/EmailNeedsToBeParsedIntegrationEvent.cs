namespace Email.EmailParser.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public class EmailNeedsToBeParsedIntegrationEvent : IntegrationEvent
    {
        public string OrganizationEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public EmailNeedsToBeParsedIntegrationEvent()
        {

        }
    }
}

