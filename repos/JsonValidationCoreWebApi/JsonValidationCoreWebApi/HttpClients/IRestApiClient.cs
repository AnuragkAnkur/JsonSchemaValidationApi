using JsonValidationCoreWebApi.Models;

namespace JsonValidationCoreWebApi.HttpClients
{
    public interface IRestApiClient
    {
        RestApiResponse GetDataFromUrl(string siteUrl);
    }
}