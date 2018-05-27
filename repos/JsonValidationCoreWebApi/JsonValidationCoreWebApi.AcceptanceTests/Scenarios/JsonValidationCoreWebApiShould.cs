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

        public JsonValidationCoreWebApiShould()
        {
            
            _fixture = new SchemaValidationApiClientFixture("http://localhost:46998/api/schemavalidator");
        }

        [Fact]
        public void ValidateJsonFrom1StWebsiteAgainstGivenRequiredTypeSchema()
        {
            var site = "https://git.io/vpg9V";
            Given.Client_Has_A_Matchig_Json_Schema<List<RequiredTypeSchema>>(_fixture);

            When.A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(_fixture, site);

            Then.Response_Status_Code_Should_Be(HttpStatusCode.Conflict, _fixture)
                .There_Should_Be_Validation_Error_In_The_Response(_fixture, ErrorType.Required);
        }

        [Fact]
        public void ValidateJsonFrom1StWebsiteAgainstGivenNonRequiredTypeSchema()
        {
            var site = "https://git.io/vpg9V";
            Given.Client_Has_A_Matchig_Json_Schema<List<PropertyNotRequiredTypeSchema>>(_fixture);

            When.A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(_fixture, site);

            Then.Response_Status_Code_Should_Be(HttpStatusCode.OK, _fixture)
                .There_Should_Be_No_Error_In_The_Response(_fixture);
        }

        [Fact]
        public void ValidateJsonFrom2NdWebsiteAgainstGivenRequiredTypeSchema()
        {
            var site = "https://git.io/vpg95";
            Given.Client_Has_A_Matchig_Json_Schema<List<ExactMatchingRequiredTypeSchema>>(_fixture);

            When.A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(_fixture, site);

            Then.Response_Status_Code_Should_Be(HttpStatusCode.OK, _fixture)
                .There_Should_Be_No_Error_In_The_Response(_fixture);
        }

        [Fact]
        public void ValidateJsonFrom2NdWebsiteAgainstGivenMinLenghtTypeSchema()
        {
            var site = "https://git.io/vpg95";
            Given.Client_Has_A_Matchig_Json_Schema<List<MinLengthSchema>>(_fixture);

            When.A_Request_Is_Sent_To_Validate_JsonData_From_A_Website(_fixture, site);

            Then.Response_Status_Code_Should_Be(HttpStatusCode.Conflict, _fixture)
                .There_Should_Be_Validation_Error_In_The_Response(_fixture, ErrorType.MinimumItems);
        }
    }
}
