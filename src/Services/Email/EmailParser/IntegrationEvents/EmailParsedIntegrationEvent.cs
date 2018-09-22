namespace Email.EmailParser.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public class EmailParsedIntegrationEvent : IntegrationEvent
    {
        public Dictionary<string, string> ExtractedField { get; set; }

        public EmailParsedIntegrationEvent()
        {

        }
    }
}

