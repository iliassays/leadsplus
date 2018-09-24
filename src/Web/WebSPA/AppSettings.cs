using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadsPlus.WebSPA
{
    public class AppSettings
    {
        public string IdentityUrl { get; set; }
        public string AgentUrl { get; set; }
        public string ContactUrl { get; set; }
        public string InquiryHistoryUrl { get; set; }
        

        public string ActivateCampaignDetailFunction { get; set; }
        public bool UseCustomizationData { get; set; }
    }
}
