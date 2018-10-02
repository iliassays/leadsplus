namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class OrganizationInfo
    {
        public string OrganizationEmail { get; set; }
        public string OrganizationDomain { get; set; }
        public string OrganizationName { get; set; }

        public OrganizationInfo()
        {

        }
    }
}
