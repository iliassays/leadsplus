namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class InqueryHistory : AggregateRoot, IViewModel
    {
        public string CustomerEmail { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public string AgentEmail { get; set; }
        public string Comment { get; set; }
        public bool CustomerAutoresponderSent { get; set; }
        public bool AgentAutoresponderSent { get; set; }

        public AgentInfo AgentInfo { get; set; }

        public string AgentId { get; set; }

        public InqueryStatus InqueryStatus { get; set; }
        public InqueryParsedToken InqueryParsedToken { get; set; }
        public Dictionary<string, string> ExtractedFields { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public InqueryHistory()
        {

        }

        public InqueryHistory(string id, string customerEmail, string message, string subject, string agentEmail, AgentInfo agentInfo)
            : this()
        {
            Id = id;
            CustomerEmail = customerEmail;
            Message = message;
            Subject = subject;
            AgentEmail = agentEmail;
            this.AgentInfo = agentInfo;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;

            var inqueryHistoryStartedDomainEvent = new InqueryHistoryStartedDomainEvent
            {
                InqueryHistory = this
            };

            this.AddDomainEvent(inqueryHistoryStartedDomainEvent);
        }

        public void SetSentForParsingStatus()
        {
            if (InqueryStatus == InqueryStatus.Submitted)
            {
                AddDomainEvent(new InqueryHistoryStatusChangedToSentForParsingDomainEvent(Id));

                InqueryStatus = InqueryStatus.SentForParsing;
                Comment = "Inquery was sent to zapiar to be parsed and successfuly parsed.";
            }
        }

        public void SetParsedStatus()
        {
            if (InqueryStatus == InqueryStatus.SentForParsing)
            {
                AddDomainEvent(new InqueryHistoryStatusChangedToParsedDomainEvent(this));

                InqueryStatus = InqueryStatus.Parsed;
                Comment = "Inquery was sent to zapiar to be parsed and successfuly parsed.";
            }
        }

        public void SetAutoresponderSendingStatus()
        {
            if (InqueryStatus == InqueryStatus.AutoresponderSending && CustomerAutoresponderSent == true && AgentAutoresponderSent == true)
            {
                SetAutoresponderSendingCompletedStatus();
            }
            else if(InqueryStatus == InqueryStatus.Parsed)
            {
                AddDomainEvent(new InqueryHistoryStatusChangedToAutoresponderSendingDomainEvent(Id));

                InqueryStatus = InqueryStatus.AutoresponderSending;
                Comment = "System sending autorespondar to customer and agent.";
            }
        }

        public void SetAutoresponderSendingCompletedStatus()
        {
            if (InqueryStatus == InqueryStatus.AutoresponderSending)
            {
                AddDomainEvent(new InqueryHistoryStatusChangedToAutoresponderSendingCompletedDomainEvent(Id));

                InqueryStatus = InqueryStatus.AutoresponderSendingCompleted;
                Comment = "System sent all the autoresponder to customer and agent.";
            }

            //For now this is the last step. Let's move to SetProcessedStatus

            SetProcessedStatus();
        }

        public void SetProcessedStatus()
        {
            if (InqueryStatus == InqueryStatus.AutoresponderSending)
            {
                AddDomainEvent(new InqueryHistoryStatusChangedToProcessedDomainEvent(Id));

                InqueryStatus = InqueryStatus.Processed;
                Comment = "System completed all the activities for this inquery and turned it to processed.";
            }
        }
    }
}
