namespace InqueryHistory.Domain.Events
{
    using MediatR;
    using System;

    public class InqueryHistoryStatusChangedToParsedDomainEvent : INotification
    {
        public InqueryHistory InqueryHistory { get; }

        public InqueryHistoryStatusChangedToParsedDomainEvent(InqueryHistory inqueryHistory)
            => InqueryHistory = inqueryHistory;
    }
}
