namespace JsonValidationCoreWebApi.Validators
{
    public interface IJsonValidator
    {
        bool Validate(string jsonBody);
    }
}