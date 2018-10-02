namespace Agent.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;
    using System.Linq;    

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
        public string DataStudioUrl { get; set; }
        
        public AgentRentInquiry RentInquiry { get; set; }
        public AgentBuyInquiry BuyInquiry { get; set; }

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

        public void CreateTypeform(InquiryType typeFormType)
        {
            var agentTypeformUpdatedEvent = new AgentTypeformCreatedEvent()
            {
                Agent = this,
                TypeFormType = typeFormType
            };

            this.AddDomainEvent(agentTypeformUpdatedEvent);
        }

        public void CreateSpreadsheet(InquiryType typeFormType)
        {
            var agentSpreadsheetCreatedEvent = new AgentSpreadsheetCreatedEvent()
            {
                Agent = this,
                TypeFormType = typeFormType
            };

            this.AddDomainEvent(agentSpreadsheetCreatedEvent);
        }

        //public void AddTypeFormItem(string typeFormUrl, TypeFormType type)
        //{
        //    var existingTypeform = AgentTypeFormCollection.Where(o => o.TypeformType == type)
        //        .SingleOrDefault();

        //    if (existingTypeform != null)
        //    {
        //        existingTypeform.TypeFormUrl = typeFormUrl;
        //    }
        //    else
        //    {
        //        //add validated new order item

        //        var agentTypeForm = new AgentTypeForm(typeFormUrl, type);
        //        AgentTypeFormCollection.Add(agentTypeForm);
        //    }
        //}

        //public void LinkSpreadsheetWithTypeFormItem(string spreadsheetUrl, string spreadsheetId, string spreadsheetName, TypeFormType type)
        //{
        //    var existingTypeform = AgentTypeFormCollection.Where(o => o.TypeformType == type)
        //        .SingleOrDefault();

        //    if (existingTypeform != null)
        //    {
        //        existingTypeform.AddSpreadsheetToAgentTypeForm(spreadsheetId, spreadsheetName, spreadsheetUrl);
        //    }
        //}

        public string GetSpreadsheetName(string suffix)
        {
            return $"{this.Email}_{this.Firstname}_{this.Id}_{suffix}";
        }

        public string GetTypeformName(string suffix)
        {
            return $"{this.Email}_{this.Firstname}_{this.Id}_{suffix}";
        }
    }
}
