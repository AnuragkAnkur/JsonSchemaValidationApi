namespace JsonValidationCoreWebApi.Validators
{
    public interface IJsonValidator
    {
        bool ValidateJson(string jsonBody);

        bool ValidateJsonAgainstSchema(string validJson, string data);
    }
}