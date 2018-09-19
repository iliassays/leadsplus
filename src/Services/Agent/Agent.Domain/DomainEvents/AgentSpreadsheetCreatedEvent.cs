namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentSpreadsheetCreatedEvent : INotification
    {
        public Agent Agent { get; set; }
    }
}
