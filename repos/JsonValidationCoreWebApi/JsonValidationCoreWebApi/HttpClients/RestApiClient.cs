using System;
using System.Net.Http;
using JsonValidationCoreWebApi.Contracts.Models;

namespace JsonValidationCoreWebApi.HttpClients
{
    public class RestApiClient : IRestApiClient
    {
        public RestApiResponse GetDataFromUrl(string siteUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var uri = new Uri(siteUrl);
                var response = httpClient.GetAsync(uri).GetAwaiter().GetResult();
                var returnedJson = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                return new RestApiResponse()
                {
                    Data = returnedJson
                };
            }
        }
    }
}