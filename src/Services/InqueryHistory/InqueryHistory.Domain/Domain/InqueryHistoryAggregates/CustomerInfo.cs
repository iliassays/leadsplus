namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class CustomerInfo
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
        
        public DateTime UpdatedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public CustomerInfo()
        {

        }
    }
}
