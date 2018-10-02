namespace Agent.Domain
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class AgentAutoresponderTemplate
    {
        public string AgentAutoresponderTemplateId { get; set; }
        public string CustomerAutoresponderTemplateId { get; set; }

        public AgentAutoresponderTemplate(string agentAutoresponderTemplateId, string customerAutoresponderTemplateId)
        {
            this.AgentAutoresponderTemplateId = agentAutoresponderTemplateId;
            this.CustomerAutoresponderTemplateId = customerAutoresponderTemplateId;
        }
    }
}
