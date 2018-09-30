namespace Agent.TypeFormIntegration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class TypeFormSettings : ITypeFormSettings
    {
        public void TypeformSettings(IConfiguration configuration)
        {
            this.ApiKey = configuration.GetValue<string>("TypeformApiKey");
            this.ApiRoot = configuration.GetValue<string>("ApiRoot");
            this.TypeformApi = configuration.GetValue<string>("TypeformApi");
            //this.BuyerInquiryTemplateUrl = configuration.GetValue<string>("BuyerInquiryTemplateUrl");
            //this.RentInquiryTemplateUrl = configuration.GetValue<string>("RentInquiryTemplateUrl");
            //this.WrokSpace = configuration.GetValue<string>("WrokSpace");
        }

        public string ApiKey { get; protected set; }
        public string TypeformApi { get; private set; }

        //public string BuyerInquiryTemplateUrl { get; private set; }
        //public string RentInquiryTemplateUrl { get; private set; }
        //public string WrokSpace { get; private set; }
        public string ApiRoot { get; private set; }
    }
}

