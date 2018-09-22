namespace InqueryHistory.Domain.Events
{
    using MediatR;
    using System;

    public class InqueryHistoryStatusChangedToAutoresponderSendingCompletedDomainEvent : INotification
    {
        public string InqueryHistoryId { get; }

        public InqueryHistoryStatusChangedToAutoresponderSendingCompletedDomainEvent(string inqueryHistoryId)
            => InqueryHistoryId = inqueryHistoryId;
    }
}
