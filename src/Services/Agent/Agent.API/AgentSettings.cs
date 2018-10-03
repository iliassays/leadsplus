using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agent
{
    public class AgentSettings
    {
        public string EventBusConnection { get; set; }
        public string DatabaseConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public string TypeFormApiKey { get; set; }
        public string TypeFormApiRoot { get; set; }
    }
}
