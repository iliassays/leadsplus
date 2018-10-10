namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentBuyInquiry : AgentInquiry
    {        
        public string MortgageSpreadsheetUrl { get; set; }
        public string MortgageSpreadsheetId { get; set; }
        public string MortgageSpreadsheetName { get; set; }
        public string MortgageSpreadsheetShareableUrl { get; set; }

        public AgentBuyInquiry() : base(InquiryType.BuyInquiry)
        {
            
        }

        public void AddMortgageSpreadsheetToInquiry(string spreadsheetId, string spreadsheetName, string spreadsheetUrl)
        {
            this.MortgageSpreadsheetUrl = spreadsheetUrl;
            this.MortgageSpreadsheetId = spreadsheetId;
            this.MortgageSpreadsheetName = spreadsheetName;
        }

        public void UpdateMortgageSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl, string mortgageSpreadsheetsharableUrl)
        {
            this.MortgageSpreadsheetUrl = spreadsheetUrl;
            this.MortgageSpreadsheetId = spreadsheetId;
            this.MortgageSpreadsheetName = spreadsheetName;
            this.MortgageSpreadsheetShareableUrl = mortgageSpreadsheetsharableUrl;
        }
    }
}
