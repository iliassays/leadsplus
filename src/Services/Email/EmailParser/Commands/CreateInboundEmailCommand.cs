namespace Cloudmailin.Webhook.Command
{
    public class CreateInboundEmailCommand
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Plain { get; set; }
        public string Text { get; set; }
    }
}
