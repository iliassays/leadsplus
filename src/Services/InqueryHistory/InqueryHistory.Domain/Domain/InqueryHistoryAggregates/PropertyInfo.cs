namespace InqueryHistory.Domain
{
    using LeadsPlus.Core;
    using System;
    using System.Collections.Generic;
    using Events;

    public class PropertyInfo
    {
        public string ReferenceNo { get; set; }
        public string PropertyAddress { get; set; }
        public string PropertyUrl { get; set; }
        public string Message { get; set; }

        public PropertyInfo()
        {

        }
    }
}
