namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public enum AutoresponderTemplateType
    {
        BuyInquiry = 0,
        RentInquiry = 1,
        MortgageInquiry = 2
    }

    public class AgentAutoresponderTemplate
    {
        public string AgentAutoresponderTemplateId { get; set; }
        public string CustomerAutoresponderTemplateId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public AutoresponderTemplateType AutoresponderTemplateType { get; set; }

        public AgentAutoresponderTemplate(string agentAutoresponderTemplateId, string customerAutoresponderTemplateId, AutoresponderTemplateType templateType)
        {
            this.AgentAutoresponderTemplateId = agentAutoresponderTemplateId;
            this.CustomerAutoresponderTemplateId = customerAutoresponderTemplateId;
            this.AutoresponderTemplateType = templateType;
        }
    }
}
