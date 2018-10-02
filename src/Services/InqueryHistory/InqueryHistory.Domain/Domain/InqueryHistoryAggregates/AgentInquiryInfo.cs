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

        public AgentAutoresponderTemplateInfo AgentAutoresponderTemplateInfo { get; set; }

        public AgentInquiryInfo()
        {
            AgentAutoresponderTemplateInfo = new AgentAutoresponderTemplateInfo();
        }
    }
}
