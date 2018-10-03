namespace Agent.TypeFormIntegration
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Options;

    public class TypeFormSettings : ITypeFormSettings
    {
        public TypeFormSettings(IConfiguration configuration)
        {
            this.ApiKey = configuration.GetValue<string>("TypeFormApiKey");
            this.ApiRoot = configuration.GetValue<string>("TypeFormApiRoot");
            //this.TypeFormApi = configuration.GetValue<string>("TypeFormApi");
            //this.BuyerInquiryTemplateUrl = configuration.GetValue<string>("BuyerInquiryTemplateUrl");
            //this.RentInquiryTemplateUrl = configuration.GetValue<string>("RentInquiryTemplateUrl");
            //this.WrokSpace = configuration.GetValue<string>("WrokSpace");
        }

        public string ApiKey { get; protected set; }
        //public string TypeFormApi { get; private set; }

        //public string BuyerInquiryTemplateUrl { get; private set; }
        //public string RentInquiryTemplateUrl { get; private set; }
        //public string WrokSpace { get; private set; }
        public string ApiRoot { get; private set; }
    }
}

