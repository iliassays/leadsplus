namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentMailboxCreatedEvent : INotification
    {
        public Agent Agent { get; set; }
    }
}
