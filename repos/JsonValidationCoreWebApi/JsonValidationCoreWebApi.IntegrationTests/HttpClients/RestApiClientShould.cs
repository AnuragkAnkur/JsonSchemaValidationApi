using JsonValidationCoreWebApi.HttpClients;
using Xunit;

namespace JsonValidationCoreWebApi.IntegrationTests.RestApi
{
    public class RestApiClientShould
    {
        private readonly RestApiClient _client;

        public RestApiClientShould()
        {
            _client = new RestApiClient();
        }

        [Fact]
        public void Get_A_Json_Response_From_A_Rest_Api()
        {
            var siteUrl = "https://git.io/vpg9V";

            var response = _client.GetDataFromUrl(siteUrl);

            Assert.NotNull(response);
            Assert.NotEmpty(response.Data);
        }
    }
}
