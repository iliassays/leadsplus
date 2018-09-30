namespace InqueryHistory.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentAutoresponderTemplateInfo
    {
        public string AgentAutoresponderTemplateId { get; set; }
        public string CustomerAutoresponderTemplateId { get; set; }

        public AgentAutoresponderTemplateInfo()
        {
            
        }
    }
}
