﻿namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentSocialMediaUpdatedEvent : INotification
    {
        public Agent Agent { get; set; }
    }
}
