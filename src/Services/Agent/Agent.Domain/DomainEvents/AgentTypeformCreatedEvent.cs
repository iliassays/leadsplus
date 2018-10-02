namespace Agent.Domain.Events
{
    using MediatR;
    using System;

    public class AgentTypeformCreatedEvent : INotification
    {
        public InquiryType TypeFormType { get; set; }
        public Agent Agent { get; set; }
    }
}
