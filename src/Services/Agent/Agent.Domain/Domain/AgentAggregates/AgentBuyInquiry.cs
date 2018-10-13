namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentBuyInquiry : AgentInquiry
    {      
        public AgentBuyInquiry() : base(InquiryType.BuyInquiry)
        {
            
        }
    }
}
