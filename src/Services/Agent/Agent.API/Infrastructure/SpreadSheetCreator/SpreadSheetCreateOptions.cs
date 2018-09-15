using System.Collections.Generic;

namespace Agent.SpreadSheetIntegration
{
    public class SpreadSheetCreateOptions
    {
        public string SpreadSheetName;
        public string WorkSheetName;
        public IList<object> HeaderValues;
        public IList<object> InitialValues;

        public SpreadSheetCreateOptions()
        {
            HeaderValues = new List<object>();
            InitialValues = new List<object>();
        }
    }
}
