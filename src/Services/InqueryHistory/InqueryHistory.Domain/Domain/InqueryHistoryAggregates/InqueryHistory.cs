namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    public enum InquiryType
    {
        BuyInquiry = 0,
        RentInquiry = 1,
        MortgageInquiry = 2
    }

    public enum InqueryStatus
    {
        Submitted = 0,
        SentForParsing = 1,
        Parsed = 2,
        AutoresponderSending = 3,
        AutoresponderSendingCompleted = 4,
        Processed = 5,
        Failed = 6
    }

    public class InqueryHistory : AggregateRoot, IViewModel
    {
        //public string OrganizationEmail { get; set; }
        public OrganizationInfo OrganizationInfo { get; set; }
        //public string CustomerEmail { get; set; }
        public string Message { get; set; }
        public string MessagePlainText { get; set; }
        public string Subject { get; set; }
        public string AgentEmail { get; set; }
        public string Comment { get; set; }
        public bool CustomerAutoresponderSent { get; set; }
        public bool AgentAutoresponderSent { get; set; }

        public AgentInfo AgentInfo { get; set; }
        public AgentInquiryInfo AgentInquiryInfo { get; set; }
        public AgentAutoresponderTemplateInfo AgentAutoresponderTemplateInfo { get; set; }
        public CustomerInfo CustomerInfo { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public string AgentId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public InqueryStatus InqueryStatus { get; set; }

        [BsonRepresentation(BsonType.String)]
        public InquiryType InquiryType { get; set; }

        public InqueryParsedToken InqueryParsedToken { get; set; }
        public Dictionary<string, string> ExtractedFields { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public InqueryHistory()
        {

        }

        public InqueryHistory(string id, InquiryType inquiryType, string messagePlainText, string message, string subject, 
            string agentEmail, AgentInfo agentInfo, AgentInquiryInfo agentInquiryInfo, 
            AgentAutoresponderTemplateInfo agentAutoresponderTemplateInfo, OrganizationInfo organizationInfo)
            : this()
        {
            Id = id;
            //OrganizationEmail = organizationEmail;
            //Organization = organizationEmail?.Split("@")[1];
            Message = message;
            MessagePlainText = messagePlainText;
            Subject = subject;
            AgentEmail = agentEmail;
            AgentInfo = agentInfo;
            InquiryType = inquiryType;
            
            OrganizationInfo = organizationInfo;
            AgentInquiryInfo = agentInquiryInfo;
            AgentAutoresponderTemplateInfo = agentAutoresponderTemplateInfo;

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
            if (InqueryStatus == InqueryStatus.AutoresponderSendingCompleted)
            {
                AddDomainEvent(new InqueryHistoryStatusChangedToProcessedDomainEvent(Id));

                InqueryStatus = InqueryStatus.Processed;
                Comment = "System completed all the activities for this inquery and turned it to processed.";
            }
        }

        public string GenerateTypeFormLink(string baseLink)
        {
            return $"{baseLink}?id={Id}&cfn={CustomerInfo.Firstname}&cln={CustomerInfo.Lastname}";
        }
    }
}
