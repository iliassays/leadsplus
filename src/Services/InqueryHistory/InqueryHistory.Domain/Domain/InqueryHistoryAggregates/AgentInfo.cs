namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class AgentInfo : AggregateRoot, IViewModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string IntegrationEmail { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }

        public AgentTypeFormInfo AgentTypeFormInfo { get; set; }

        public AgentAutoresponderTemplateInfo AgentAutoresponderTemplateInfo { get; set; }

        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public AgentInfo()
        {

        }
    }
}
