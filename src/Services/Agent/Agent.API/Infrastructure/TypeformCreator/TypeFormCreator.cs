using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Agent.TypeFormIntegration
{
    public static class EndPoints
    {
        public const string Root = "https://api.typeform.com/";
        public const string TemplateUri = "https://api.typeform.com/forms/KUB34m";
        public const string Account = "me";
        public const string Images = "images";
        public const string Themes = "themes";
        public const string Create = "forms";
        public const string WrokSpace = "workspaces";
        public const string Token = "91KESJeJGFNbxVrJaSh9F3nxpg4nNKr9qACKMyXGei69";
    }

    public class TypeFormCreator
    {
        public TypeFormCreator()
        {
           
        }

        public static async Task<string> GetTemplateFormAsync()
        {
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler))
                {
                    AttatchAuthHeader(httpClient);

                    var req = CreateRequest();
                    var response = await httpClient.GetAsync(new Uri(EndPoints.TemplateUri));

                    return response.Content.ReadAsStringAsync().Result;
                }
            }
        }

        public static async Task<string> CreateTypeFormAsync(string typeForm)
        {
            var uri = $"{EndPoints.Root}{EndPoints.Create}";

            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler) { BaseAddress = new Uri(uri) })
                {
                    AttatchAuthHeader(httpClient);
                    var req = CreateRequest();
                    
                    req.Content = CreateContent(typeForm);

                    var response = await SendRequestAsync(uri, httpClient, req);
                    return response;
                }
            }
        }

        private static async Task<string> SendRequestAsync(string uri, HttpClient httpClient, HttpRequestMessage req)
        {
            var res = await httpClient.SendAsync(req);
            var strings = await res.Content.ReadAsStringAsync();

            return strings;
        }

        private static HttpRequestMessage CreateRequest()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };

            return request;
        }

        private static StringContent CreateContent(string typeFormContent)
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

            Regex reg = new Regex(@"\""(id\"":[ ]?\""[\d\s\w]*\"",)");

            typeFormContent = reg.Replace(typeFormContent, delegate (Match m) {
                return string.Empty;
            });

            //update title

            var content = new StringContent(typeFormContent, Encoding.UTF8, "application/json");
            return content;
        }

        private static void AttatchAuthHeader(HttpClient httpClient)
            => httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", EndPoints.Token);
    }
}
