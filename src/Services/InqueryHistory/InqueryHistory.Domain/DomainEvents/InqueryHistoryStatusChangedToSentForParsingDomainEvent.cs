namespace InqueryHistory.Domain.Events
{
    using MediatR;
    using System;

    public class InqueryHistoryStatusChangedToSentForParsingDomainEvent : INotification
    {
        public string InqueryHistoryId { get; }

        public InqueryHistoryStatusChangedToSentForParsingDomainEvent(string inqueryHistoryId)
            => InqueryHistoryId = inqueryHistoryId;
    }
}
