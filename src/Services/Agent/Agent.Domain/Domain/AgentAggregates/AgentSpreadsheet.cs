namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentSpreadsheet
    {
        public string MortgageSpreadsheetUrl { get; set; }
        public string MortgageSpreadsheetId { get; set; }
        public string MortgageSpreadsheetName { get; set; }
        public string MortgageSpreadsheetShareableUrl { get; set; }

        public string LandlordSpreadsheetUrl { get; set; }
        public string LandlordSpreadsheetId { get; set; }
        public string LandlordSpreadsheetName { get; set; }
        public string LandlordSpreadsheetShareableUrl { get; set; }

        public string VendorSpreadsheetUrl { get; set; }
        public string VendorSpreadsheetId { get; set; }
        public string VendorSpreadsheetName { get; set; }
        public string VendorSpreadsheetShareableUrl { get; set; }

        public string AggregateSpreadsheetUrl { get; set; }
        public string AggregateSpreadsheetId { get; set; }
        public string AggregateSpreadsheetName { get; set; }
        public string AggregateSpreadsheetShareableUrl { get; set; }

        public AgentSpreadsheet()
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

        public void AddLandLoardSpreadsheetToInquiry(string spreadsheetId, string spreadsheetName, string spreadsheetUrl)
        {
            LandlordSpreadsheetUrl = spreadsheetUrl;
            LandlordSpreadsheetId = spreadsheetId;
            LandlordSpreadsheetName = spreadsheetName;
        }

       

        public void UpdateLandlordSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl, string landlordspreadsheetShareableUrl)
        {
            LandlordSpreadsheetShareableUrl = landlordspreadsheetShareableUrl;
            LandlordSpreadsheetUrl = spreadsheetUrl;
            LandlordSpreadsheetId = spreadsheetId;
            LandlordSpreadsheetName = spreadsheetName;
        }

        public void AddVendorSpreadsheetToInquiry(string spreadsheetId, string spreadsheetName, string spreadsheetUrl)
        {
            LandlordSpreadsheetUrl = spreadsheetUrl;
            LandlordSpreadsheetId = spreadsheetId;
            LandlordSpreadsheetName = spreadsheetName;
        }

        public void UpdateVendorSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl, string shareableUrl)
        {
            VendorSpreadsheetShareableUrl = shareableUrl;
            VendorSpreadsheetUrl = spreadsheetUrl;
            VendorSpreadsheetId = spreadsheetId;
            VendorSpreadsheetName = spreadsheetName;
        }

        public void AddAggregateSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl)
        {
            AggregateSpreadsheetUrl = spreadsheetUrl;
            AggregateSpreadsheetId = spreadsheetId;
            AggregateSpreadsheetName = spreadsheetName;
        }

        public void UpdateAggregateSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl, string shareableUrl)
        {
            AggregateSpreadsheetShareableUrl = shareableUrl;
            AggregateSpreadsheetUrl = spreadsheetUrl;
            AggregateSpreadsheetId = spreadsheetId;
            AggregateSpreadsheetName = spreadsheetName;
        }
    }
}
