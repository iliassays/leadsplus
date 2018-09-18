using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Email.SmtpService.EmailSender
{
    public class SendGridSettings : ISendGridSettings
    {
        public SendGridSettings(IConfiguration configuration, IOptions<Settings> settings)
        {
            this.ApiKey = configuration.GetValue<string>("SendgridApiKey");
            this.SendgridMailingApi = configuration.GetValue<string>("SendgridMailingApi");
        }

        public string ApiKey { get; protected set; }
        public string SendgridMailingApi { get; private set; }
    }
}

