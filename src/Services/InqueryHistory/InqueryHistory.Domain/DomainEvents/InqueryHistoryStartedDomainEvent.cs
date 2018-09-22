namespace InqueryHistory.Domain.Events
{
    using MediatR;
    using System;

    public class InqueryHistoryStartedDomainEvent : INotification
    {
        public InqueryHistory InqueryHistory { get; set; }
    }
}
