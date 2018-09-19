namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentTypeformCreatedEvent : INotification
    {
        public Agent Agent { get; set; }
    }
}
