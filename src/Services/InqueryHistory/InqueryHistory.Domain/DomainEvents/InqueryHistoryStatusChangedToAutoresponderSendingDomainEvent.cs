namespace InqueryHistory.Domain.Events
{
    using MediatR;
    using System;

    public class InqueryHistoryStatusChangedToAutoresponderSendingDomainEvent : INotification
    {
        public string InqueryHistoryId { get; }

        public InqueryHistoryStatusChangedToAutoresponderSendingDomainEvent(string inqueryHistoryId)
            => InqueryHistoryId = inqueryHistoryId;
    }
}
