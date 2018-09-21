namespace Email.SmtpService.EmailSender
{
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class SendGridEmailMessageSender : IEmailMessageSender
    {
        private readonly ISendGridSettings sendGridSettings;

        public SendGridEmailMessageSender(ISendGridSettings sendGridSettings)
        {
            this.sendGridSettings = sendGridSettings;
        }

        public async Task SendEmail(EmailSendingRequest emailSendingRequest)
        {
            var sendgridData = new SendgridData()
            {
                Personalizations = new List<Personalization>()
                {
                    new Personalization
                    {
                        To = new List<To>() {
                            new To {
                                Email = emailSendingRequest.Recipient.Email,
                                Name =  emailSendingRequest.Recipient.Name
                            }
                        },
                        Subject = emailSendingRequest.EmailMessage.Subject,
                        Substitutions = emailSendingRequest.MergeFields
                    }
                },
                Content = new List<Content>() { new Content { Type = "text/html", Value = emailSendingRequest.EmailMessage.Body } },
                From = new From
                {
                    Email = emailSendingRequest.Sender.Email,
                    Name = emailSendingRequest.Sender.FromLabel
                },
                ReplyTo = new ReplyTo
                {
                    Email = emailSendingRequest.Sender.Email,
                    Name = emailSendingRequest.Sender.FromLabel
                },
                TemplateId = emailSendingRequest.TemplateId,
                
            };

            if (!string.IsNullOrEmpty(emailSendingRequest.TemplateId))
            {
                sendgridData.Content = null;
                sendgridData.Personalizations[0].Subject = string.Empty;

                await SendEmail(sendgridData);
            }
            else
            {
                await SendEmail(sendgridData);
            }
        }

        public async Task SendEmail(SendgridData sendgridData)
        {
            //WTF with this approach??
            //using (var httpClient = new HttpClient() { })
            //{
            //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sendGridSettings.ApiKey);

            //    await httpClient.PostAsJsonAsync(sendGridSettings.SendgridMailingApi, JsonConvert.SerializeObject(sendgridData))
            //              .ContinueWith((postTask) => postTask.Result.EnsureSuccessStatusCode());

            //}

            var data = JsonConvert.SerializeObject(sendgridData);
            var client = new RestClient(sendGridSettings.SendgridMailingApi);
            var request = new RestRequest(Method.POST);

            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", $"Bearer {sendGridSettings.ApiKey}");
            request.AddParameter("application/json", JsonConvert.SerializeObject(sendgridData), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);
        }
    }

    public class SendgridData
    {
        [JsonProperty("personalizations")]
        public List<Personalization> Personalizations { get; set; }

        [JsonProperty("from")]
        public From From { get; set; }

        [JsonProperty("reply_to")]
        public ReplyTo ReplyTo { get; set; }

        [JsonProperty("template_id")]
        public string TemplateId { get; set; }

        [JsonProperty("content")]
        public List<Content> Content { get; set; }
    }

    public class Personalization
    {
        [JsonProperty("to")]
        public List<To> To { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("substitutions")]
        public Dictionary<string, string> Substitutions { get; set; }
    }

    public class To
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class From
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class ReplyTo
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }
}

