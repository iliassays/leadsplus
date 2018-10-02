namespace Contact.Domain
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Events;
    using LeadsPlus.Core;

    public class Contact : AggregateRoot, IViewModel
    {        
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Aboutme { get; set; }

        public string CreatedBy { get; set; }
        public string Source { get; set; }
        public string CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        private bool _isDraft;

        public static Contact NewDraft()
        {
            var contact = new Contact();
            contact._isDraft = true;

            return contact;
        }

        protected Contact()
        {
            _isDraft = false;
        }

        public Contact(string id, string ownerId, string ownerName, string source, string firstname, string lastname, string email, string company,
            string phone, string address, string city, string country, string aboutme) : this()
        {
            Id = id;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Country = country;
            Phone = phone;
            Address = address;
            City = city;
            Country = country;
            Aboutme = aboutme;

            CreatedBy = ownerId;
            CreatedByName = ownerName;

            Source = source;
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;

            var contactCreatedDomainEvent = new ContactCreatedDomainEvent
            {
                Email = email,
                Firstname = firstname,
                Lastname = lastname,
                CreatedBy = CreatedBy
            };

            this.AddDomainEvent(contactCreatedDomainEvent);
        }
    }
}

