﻿namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentRentInquiry : AgentInquiry
    {        
        public string LandlordSpreadsheetUrl { get; set; }
        public string LandlordSpreadsheetId { get; set; }
        public string LandlordSpreadsheetName { get; set; }
        public string LandlordSpreadsheetShareableUrl { get; set; }

        public AgentRentInquiry() : base(InquiryType.RentInquiry)
        {
            
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
    }
}
