namespace InqueryHistory.Domain.Events
{
    using MediatR;
    using System;

    public class InqueryHistoryStatusChangedToProcessedDomainEvent : INotification
    {
        public string InqueryHistoryId { get; }

        public InqueryHistoryStatusChangedToProcessedDomainEvent(string inqueryHistoryId)
            => InqueryHistoryId = inqueryHistoryId;
    }
}
