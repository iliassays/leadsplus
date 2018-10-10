namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentLogoUpdatedEvent : INotification
    {
        public Agent Agent { get; set; }
    }
}
