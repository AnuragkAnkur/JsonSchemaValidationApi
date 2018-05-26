using System.Collections.Generic;
using System.IO;
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

        public IList<ValidationError> ValidateJsonAgainstSchema(string jsonSchema, string data)
        {
            var schema = JSchema.Parse(jsonSchema.Trim());

            var jsonData = JToken.Parse(data.Trim());

            IList<ValidationError> errorList = new List<ValidationError>();

            jsonData.IsValid(schema, out errorList);

            return errorList;
        }
    }
}