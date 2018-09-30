namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class AgentTypeFormInfo
    {
        public string TypeFormUrl { get; set; }
        public string SpreadsheetUrl { get; set; }
        public string SpreadsheetId { get; set; }
        public string SpreadsheetName { get; set; }

        public AgentTypeFormInfo()
        {

        }
    }
}
