namespace Email.EmailParser.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public class EmailParsedIntegrationEvent : IntegrationEvent
    {
        public Dictionary<string, string> ExtractedFields { get; set; }

        public EmailParsedIntegrationEvent()
        {

        }
    }
}

