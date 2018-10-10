namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentMarkedAsLaunchedEvent : INotification
    {
        public Agent Agent { get; set; }
    }
}
