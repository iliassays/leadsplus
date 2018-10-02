namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentSpreadsheetCreatedEvent : INotification
    {
        public InquiryType TypeFormType { get; set; }
        public Agent Agent { get; set; }
    }
}
