namespace InqueryHistory.IntegrationEvents
{
    using LeadsPlus.BuildingBlocks.EventBus.Events;

    public class NewInqueryRequestReceivedIntegrationEvent : IntegrationEvent
    {
        public string Body { get; set; }
        public string Subject { get; set; }
        public string OrganizationEmail { get; set; }
        public string AgentEmail { get; set; }
        public string PlainText { get; set; }

        public int InquiryType { get; set; }

        public AgentInfo AgentInfo { get; set; }
        public AgentInquiryInfo AgentInquiryInfo { get; set; }
        public AgentAutoresponderTemplateInfo AgentAutoresponderTemplateInfo { get; set; }

        public NewInqueryRequestReceivedIntegrationEvent()
        {
            this.AgentInfo = new AgentInfo();
            this.AgentInquiryInfo = new AgentInquiryInfo();
            this.AgentAutoresponderTemplateInfo = new AgentAutoresponderTemplateInfo();
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
        public string Logo { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
        public string LinkedIn { get; set; }

        public AgentInfo()
        {

        }
    }

    public class AgentInquiryInfo
    {
        public string TypeFormUrl { get; set; }
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string MortgageShareableUrl { get; set; }
        public string LandlordShareableUrl { get; set; }
        public string VendorShareableUrl { get; set; }
        public string AggregateShareableUrl { get; set; }
        public string AggregateShareableId { get; set; }

        public AgentInquiryInfo()
        {

        }
    }

    public class AgentAutoresponderTemplateInfo
    {
        public string AgentAutoresponderTemplateId { get; set; }
        public string CustomerAutoresponderTemplateId { get; set; }
    }
}
