namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentRentInquiry : AgentInquiry
    {        
        public AgentRentInquiry() : base(InquiryType.RentInquiry)
        {
            
        }
    }
}
