﻿namespace Agent.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;

    //public enum InquiryType
    //{
    //    BuyInquiry = 0,
    //    RentInquiry = 1,
    //    MortgageInquiry = 2
    //}

    public class NewInqueryRequestReceivedIntegrationEvent : IntegrationEvent
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string OrganizationEmail { get; set; }
        public string AgentEmail { get; set; }
        public string PlainText { get; set; }

        public int InquiryType { get; set; }

        public AgentInfo AgentInfo { get; set; }

        public NewInqueryRequestReceivedIntegrationEvent()
        {
            this.AgentInfo = new AgentInfo();
        }
    }

    public class AgentInfo
    {
        public string Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string IntegrationEmail { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }


        public AgentInquiryInfo AgentInquiryInfo { get; set; }

        public AgentInfo()
        {
            this.AgentInquiryInfo = new AgentInquiryInfo();
        }
    }

    public class AgentInquiryInfo
    {
        public string TypeFormUrl { get; set; }
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }

        public AgentAutoresponderTemplateInfo AgentAutoresponderTemplateInfo { get; set; }

        public AgentInquiryInfo()
        {
            this.AgentAutoresponderTemplateInfo = new AgentAutoresponderTemplateInfo();
        }
    }

    public class AgentAutoresponderTemplateInfo
    {
        public string AgentAutoresponderTemplateId { get; set; }
        public string CustomerAutoresponderTemplateId { get; set; }
    }
}
