using System.Linq;
using System.Net;
using JsonValidationCoreWebApi.AcceptanceTests.Fixtures;
using JsonValidationCoreWebApi.Contracts.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Xunit;

namespace JsonValidationCoreWebApi.AcceptanceTests.GWT
{
    public class Then
    {
        public void There_Should_Be_No_Error_In_The_Response(
            SchemaValidationApiClientFixture fixture)
        {
            var deserialisedResponse = JsonConvert.DeserializeObject<SchemaValidationResult>(fixture.ResponseContent);
            Assert.False(deserialisedResponse.SchemaValidationErrors.Any());
        }

        public static Then Response_Status_Code_Should_Be(HttpStatusCode expectedStatusCode, SchemaValidationApiClientFixture fixture)
        {
            Assert.Equal(fixture.ResponseStatusCode, expectedStatusCode);
            return new Then();
        }

        public void There_Should_Be_Validation_Error_In_The_Response(SchemaValidationApiClientFixture fixture, ErrorType type)
        {
            var deserialisedResponse = JsonConvert.DeserializeObject<SchemaValidationResult>(fixture.ResponseContent);
            Assert.True(deserialisedResponse.SchemaValidationErrors.All(x => x.ErrorType == type));
            Assert.True(deserialisedResponse.SchemaValidationErrors.Any());
        }
    }
}
