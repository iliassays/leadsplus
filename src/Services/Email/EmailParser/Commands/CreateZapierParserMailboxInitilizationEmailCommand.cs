namespace Cloudmailin.Webhook.Command
{
    public class CreateZapierParserMailboxInitilizationEmailCommand
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
