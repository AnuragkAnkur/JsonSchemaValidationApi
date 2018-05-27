using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace JsonValidationCoreWebApi.AcceptanceTests.Fixtures
{
    public class SchemaValidationApiClientFixture
    {
        private readonly Uri _uri;

        public string Schema { get; set; }

        public string ResponseContent { get; private set; }

        public HttpStatusCode ResponseStatusCode { get; private set; }

        public SchemaValidationApiClientFixture(string uri)
        {
            _uri = new Uri(uri);
        }

        public void ValidateJson(string siteUrl)
        {
            using (var client = new HttpClient())
            {
                var content = new
                {
                    Site = siteUrl,
                    Schema
                };

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var serialisedContent = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(serialisedContent, Encoding.UTF8,"application/json");

                var response = client.PostAsync(_uri, stringContent).GetAwaiter().GetResult();

                ResponseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().Trim();

                ResponseStatusCode = response.StatusCode;
            }
        }
    }
}