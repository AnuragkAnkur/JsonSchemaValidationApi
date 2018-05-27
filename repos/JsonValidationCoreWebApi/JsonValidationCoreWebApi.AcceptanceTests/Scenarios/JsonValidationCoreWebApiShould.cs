using System.Collections.Generic;
using System.Net;
using JsonValidationCoreWebApi.AcceptanceTests.Fixtures;
using JsonValidationCoreWebApi.AcceptanceTests.GWT;
using JsonValidationCoreWebApi.AcceptanceTests.Schemas;
using Newtonsoft.Json.Schema;
using Xunit;

namespace JsonValidationCoreWebApi.AcceptanceTests.Scenarios
{
    public class JsonValidationCoreWebApiShould
    {
        private readonly SchemaValidationApiClientFixture _fixture;
        private readonly string _site;

        public JsonValidationCoreWebApiShould()
        {
            _site = "https://git.io/vpg9V";
            _fixture = new SchemaValidationApiClientFixture("http://localhost:46998/api/schemavalidator");
        }

        [Fact]
        public void ValidateJsonAgainstGivenRequiredTypeSchemaFromAWebSite()
        {
            Given.I_Have_A_Matchig_Json_Schema<List<RequiredTypeSchema>>(_fixture);

            When.A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(_fixture, _site);

            Then.Response_Status_Code_Should_Be(HttpStatusCode.Conflict, _fixture)
                .There_Should_Be_Validation_Error_In_The_Response(_fixture, ErrorType.Required);
        }

        [Fact]
        public void ValidateJsonAgainstGivenNonRequiredTypeSchemaFromAWebSite()
        {
            Given.I_Have_A_Matchig_Json_Schema<List<PropertyNotRequiredTypeSchema>>(_fixture);

            When.A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(_fixture, _site);

            Then.Response_Status_Code_Should_Be(HttpStatusCode.OK, _fixture)
                .There_Should_Be_No_Error_In_The_Response(_fixture);
        }
    }
}
