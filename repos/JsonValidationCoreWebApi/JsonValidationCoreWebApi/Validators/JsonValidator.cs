using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public bool Validate(string jsonBody)
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
    }
}