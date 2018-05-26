using System.Collections.Generic;
using JsonValidationCoreWebApi.Models;

namespace JsonValidationCoreWebApi.Validators
{
    public interface IJsonValidator
    {
        bool ValidateJson(string jsonBody);

        SchemaValidationResult ValidateJsonAgainstSchema(string jsonSchema, string data);
    }
}