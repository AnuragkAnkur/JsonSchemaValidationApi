using JsonValidationCoreWebApi.AcceptanceTests.Fixtures;
using JsonValidationCoreWebApi.AcceptanceTests.Scenarios;

namespace JsonValidationCoreWebApi.AcceptanceTests.GWT
{
    public class When
    {
        public static void A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(SchemaValidationApiClientFixture fixture, string site)
        {
            fixture.ValidateJson(site);
        }
    }
}
