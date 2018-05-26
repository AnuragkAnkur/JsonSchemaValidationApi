using System.Collections.Generic;
using Newtonsoft.Json.Schema;

namespace JsonValidationCoreWebApi.Validators
{
    public interface IJsonValidator
    {
        bool ValidateJson(string jsonBody);

        IList<ValidationError> ValidateJsonAgainstSchema(string jsonSchema, string data);
    }
}