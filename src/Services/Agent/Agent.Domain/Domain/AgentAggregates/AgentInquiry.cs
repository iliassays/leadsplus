namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public enum InquiryType
    {
        BuyInquiry = 0,
        RentInquiry = 1,
        MortgageLeads = 2,
        LandlordLeads = 3,
        VendorLeads = 4,
        Aggregate = 6,
    }

    public class AgentInquiry
    {        
        public string TypeFormUrl { get; set; }
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string SpreadsheetShareableUrl { get; set; }

        public AgentAutoresponderTemplate InquiryAutoresponderTemplate { get; set; }

        [BsonRepresentation(BsonType.String)]
        public InquiryType InquiryType { get; set; }

        public AgentInquiry(InquiryType inquiryType)
        {
            this.InquiryType = inquiryType;

            this.InquiryAutoresponderTemplate = new AgentAutoresponderTemplate();
        }

        public void UpdateTypeform(string typeFormUrl)
        {
            this.TypeFormUrl = typeFormUrl;
        }

        public void AddSpreadsheetToInquiry(string spreadsheetId, string spreadsheetName, string spreadsheetUrl)
        {
            this.SpreadsheetUrl = spreadsheetUrl;
            this.SpreadsheetId = spreadsheetId;
            this.SpreadsheetName = spreadsheetName;
        }

        public void UpdateSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl, string spreadsheetShareableUrl)
        {
            this.SpreadsheetUrl = spreadsheetUrl;
            this.SpreadsheetId = spreadsheetId;
            this.SpreadsheetName = spreadsheetName;
            this.SpreadsheetShareableUrl = spreadsheetShareableUrl;
        }
    }
}
