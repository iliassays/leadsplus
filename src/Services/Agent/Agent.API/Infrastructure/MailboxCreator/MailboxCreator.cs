namespace Agent.EmailCreator
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    public class EmailAccount
    {
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Quota { get; set; }
    }

    public class MailboxCreator
    {
        public const string token = "4h5AaQ4hbPmaLhfdYiaZVdyjtkrT2279FoSFxbA735eN";
        public readonly EmailAccount emailAccount;


        public MailboxCreator(EmailAccount emailAccount)
        {
            this.emailAccount = emailAccount;
        }

        public async Task<string> Create()
        {
            await GetWithBasicAuthAsync(GetMailboxUrl(emailAccount));

            return $"{emailAccount.UserName}@{emailAccount.Domain}" ;
        }

        private string GetMailboxUrl(EmailAccount emailAccount)
        {
            var cpRoot = $"https://{emailAccount.Domain}:2083/json-api/cpanel?cpanel_jsonapi_user=user&cpanel_jsonapi_apiversion=2";
            var emailmdule = "&cpanel_jsonapi_module=Email&cpanel_jsonapi_func=addpop";
            var emailaccountBody = $"&domain={emailAccount.Domain}&email={emailAccount.UserName}"
                + $"&password={emailAccount.Password}&quota={emailAccount.Quota}";

            return cpRoot + emailmdule + emailaccountBody;
        }

        public async Task<HttpResponseMessage> GetWithBasicAuthAsync(string uri)
        {
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler) { BaseAddress = new Uri(uri) })
                {
                    AttatchBasicAuthHeader(httpClient);
                    var req = CreateGetRequest();
                    var response = await SenRequestAsync(uri, httpClient, req);

                    return response;
                }
            }
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler) { BaseAddress = new Uri(uri) })
                {
                    AttatchAuthHeader(httpClient);
                    var req = CreateGetRequest();
                    var response = await SenRequestAsync(uri, httpClient, req);
                    return response;
                }
            }
        }

        private async Task<HttpResponseMessage> SenRequestAsync(string uri, HttpClient httpClient, HttpRequestMessage req)
        {
            var res = await httpClient.SendAsync(req);

            return res;
        }

        private static void AttatchAuthHeader(HttpClient httpClient)
            => httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        private static void AttatchBasicAuthHeader(HttpClient httpClient)
        {
            var byteArray = Encoding.ASCII.GetBytes("adfenixlead:AhLmAnNvIVe8");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private static HttpRequestMessage CreateGetRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get
            };

            return request;
        }
    }
}
