using System.Collections.Generic;
using System.IO;
using JsonValidationCoreWebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Serilog;

namespace JsonValidationCoreWebApi.Validators
{
    public class JsonValidator : IJsonValidator
    {
        private readonly ILogger _logger;

        public JsonValidator(ILogger logger)
        {
            _logger = logger
                        .ForContext<JsonValidator>();
        }

        public bool ValidateJson(string jsonBody)
        {
            if (string.IsNullOrEmpty(jsonBody))
            {
                return false;
            }

            var trimmedJson = jsonBody.Trim();
            
            try
            {
                JToken.Parse(trimmedJson);
            }
            catch (JsonReaderException exception)
            {
                _logger.Error(exception, "Provided string is an invalid json");
                return false;
            }

            return true;
        }

        public IList<SchemaValidationError> ValidateJsonAgainstSchema(string jsonSchema, string data)
        {
            var schema = JSchema.Parse(jsonSchema.Trim());

            var jsonData = JToken.Parse(data.Trim());

            IList<ValidationError> validationErrors = new List<ValidationError>();

            jsonData.IsValid(schema, out validationErrors);

            var listOfErrors = new List<SchemaValidationError>();
            foreach (var error in validationErrors)
            {
                if (error.ErrorType == ErrorType.Type)
                {
                    if (error.Value == null)
                    {
                        _logger.Warning($"Encountered 'Null' value at line number {error.LineNumber}" +
                                        $"and position {error.LinePosition}.Error Message: {error.Message}");
                        continue;
                    }
                }

                listOfErrors.Add(new SchemaValidationError()
                {
                    ErrorType = error.ErrorType,
                    Message = error.Message,
                    Value = error.Value
                });
            }

            return listOfErrors;
        }
    }
}