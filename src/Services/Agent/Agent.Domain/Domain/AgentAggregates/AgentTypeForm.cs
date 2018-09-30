namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public enum TypeFormType
    {
        BuyInquiry = 0,
        RentInquiry = 1,
        MortgageInquiry = 2
    }

    public class AgentTypeForm
    {        
        public string TypeFormUrl { get; set; }
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }

        [BsonRepresentation(BsonType.String)]
        public TypeFormType TypeformType { get; set; }

        public AgentTypeForm(string typeFormUrl, TypeFormType typeformType)
        {
            this.TypeFormUrl = typeFormUrl;
            this.TypeformType = typeformType;
        }

        public void AddSpreadsheetToAgentTypeForm(string spreadsheetId, string spreadsheetName, string spreadsheetUrl)
        {
            this.SpreadsheetUrl = spreadsheetUrl;
            this.SpreadsheetId = spreadsheetId;
            this.SpreadsheetName = spreadsheetName;
        }
    }
}
