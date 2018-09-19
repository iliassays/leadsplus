namespace Agent.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class Agent : AggregateRoot, IViewModel
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

        public AgentTypeForm AgentTypeForm { get; set; }

        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public Agent()
        {

        }
        public Agent(string id, string ownerId, string firstname, string lastname, string email, string country, string city, string phone, string address, string company)
            : this()
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Country = country;
            Phone = phone;
            Address = address;
            Company = company;

            CreatedBy = ownerId;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;

            var contactCreatedDomainEvent = new AgentCreatedDomainEvent
            {
                Agent = this
            };

            this.AddDomainEvent(contactCreatedDomainEvent);
        }

        public void CreateMailbox()
        {
            var agentMailboxCreatedEvent = new AgentMailboxCreatedEvent()
            {
                Agent = this
            };

            this.AddDomainEvent(agentMailboxCreatedEvent);
        }

        public void UpdateMailbox(string newMailboxName)
        {
            var agentMailboxUpdatedEvent = new AgentMailboxUpdatedEvent()
            {
                Agent = this,
                NewMailbox = newMailboxName
            };

            this.AddDomainEvent(agentMailboxUpdatedEvent);
        }

        public void CreateTypeform()
        {
            var agentTypeformUpdatedEvent = new AgentTypeformCreatedEvent()
            {
                Agent = this
            };

            this.AddDomainEvent(agentTypeformUpdatedEvent);
        }
        public void CreateSpreadsheet()
        {
            var agentSpreadsheetCreatedEvent = new AgentSpreadsheetCreatedEvent()
            {
                Agent = this
            };

            this.AddDomainEvent(agentSpreadsheetCreatedEvent);
        }

    }
}
