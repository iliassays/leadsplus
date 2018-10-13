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

        public void UpdateVendorSpreadsheet(string spreadsheetId, string spreadsheetName, string spreadsheetUrl, string landlordspreadsheetShareableUrl)
        {
            VendorSpreadsheetShareableUrl = landlordspreadsheetShareableUrl;
            VendorSpreadsheetUrl = spreadsheetUrl;
            VendorSpreadsheetId = spreadsheetId;
            VendorSpreadsheetName = spreadsheetName;
        }
    }
}
