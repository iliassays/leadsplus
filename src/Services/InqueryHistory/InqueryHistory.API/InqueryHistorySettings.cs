namespace InqueryHistory
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class InqueryHistorySettings
    {
        public string EventBusConnection { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
