

namespace Agent.TypeFormIntegration
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    //public static class EndPoints
    //{
    //    public const string Root = "https://api.typeform.com/";
    //    public const string BuyerTemplateUri = "https://api.typeform.com/forms/KUB34m";
    //    public const string RentTemplateUri = "https://api.typeform.com/forms/KUB34m";

    //}

    public class TypeForm : ITypeForm
    {
        //private const string Account = "me";
        //private const string Images = "images";
        //private const string Themes = "themes";
        private const string Create = "forms";
        //private const string WrokSpace = "workspaces";
        //private const string Token = "91KESJeJGFNbxVrJaSh9F3nxpg4nNKr9qACKMyXGei69";

        private readonly ITypeFormSettings typeFormSettings;

        public TypeForm(ITypeFormSettings typeFormSettings)
        {
            this.typeFormSettings = typeFormSettings;
        }

        public async Task<string> GetTypeFormAsync(string url)
        {
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler))
                {
                    AttatchAuthHeader(httpClient);

                    var req = CreateRequest();
                    var response = await httpClient.GetAsync(new Uri(url));

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public async Task<string> CreateTypeFormAsync(string typeFormContent)
        {
            var uri = $"{typeFormSettings.ApiRoot}{Create}";

            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler) { BaseAddress = new Uri(uri) })
                {
                    AttatchAuthHeader(httpClient);
                    var req = CreateRequest();
                    
                    req.Content = CreateContent(typeFormContent);

                    var response = await SendRequestAsync(uri, httpClient, req);
                    return response;
                }
            }
        }

        private async Task<string> SendRequestAsync(string uri, HttpClient httpClient, HttpRequestMessage req)
        {
            var res = await httpClient.SendAsync(req);
            var strings = await res.Content.ReadAsStringAsync();

            return strings;
        }

        private HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            return request;
        }

        private StringContent CreateContent(string typeFormContent)
        {
            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                DefaultValueHandling = DefaultValueHandling.Ignore,

            };

            //remove all ids
            Regex reg = new Regex(@"\""(id\"":[ ]?\""[\d\s\w]*\"",)");

            typeFormContent = reg.Replace(typeFormContent, delegate (Match m) {
                return string.Empty;
            });

            //update title

            var content = new StringContent(typeFormContent, Encoding.UTF8, "application/json");
            return content;
        }

        private void AttatchAuthHeader(HttpClient httpClient)
            => httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", typeFormSettings.ApiKey);
    }
}
