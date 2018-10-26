namespace Cloudmailin.Webhook.Command
{
    public class CreateDemoEnquiryEmailCommand
    {
        public string CustomerFirstname { get; set; }
        public string CustomerLastname { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerMessage { get; set; }

        public bool IsBodyhtml { get; set; }

        public string To { get; set; }
        public string From { get; set; }
    }
}
