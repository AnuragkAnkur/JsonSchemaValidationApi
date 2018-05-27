using JsonValidationCoreWebApi.AcceptanceTests.Fixtures;
using JsonValidationCoreWebApi.AcceptanceTests.Scenarios;
using Newtonsoft.Json.Schema.Generation;

namespace JsonValidationCoreWebApi.AcceptanceTests.GWT
{
    public class Given
    {
        public static void Client_Has_A_Matchig_Json_Schema<T>(SchemaValidationApiClientFixture fixture)
        {
            var schemaGenerator = new JSchemaGenerator();
            var schema = schemaGenerator.Generate(typeof(T));
            fixture.Schema = schema.ToString();
        }
    }
}
