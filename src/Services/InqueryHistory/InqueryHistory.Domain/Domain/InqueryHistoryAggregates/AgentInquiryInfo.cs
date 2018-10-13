namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class AgentInquiryInfo
    {
        public string TypeFormUrl { get; set; }
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }
        public string MortgageShareableUrl { get; set; }
        public string LandlordShareableUrl { get; set; }
        public string VendorShareableUrl { get; set; }

        public AgentInquiryInfo()
        {
            
        }
    }
}
