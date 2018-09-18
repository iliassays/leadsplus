namespace Email.SmtpService.EmailSender
{
    public interface ISendGridSettings
    {
        string ApiKey { get; }
        string SendgridMailingApi { get; }
    }
}

