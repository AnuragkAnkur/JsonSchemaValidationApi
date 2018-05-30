using System.Collections.Generic;
using System.Linq;
using JsonValidationCoreWebApi.Contracts.Models;
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

        public SchemaValidationResult ValidateJsonAgainstSchema(string jsonSchema, string data)
        {
            var schema = JSchema.Parse(jsonSchema.Trim());

            var jsonData = JToken.Parse(data.Trim());
            var parsedObjectsCount = jsonData.Children().Count();
            _logger.Information($"Number Of object parsed = {parsedObjectsCount} for schema \n {jsonSchema}");

            IList<ValidationError> validationErrors = new List<ValidationError>();

            jsonData.IsValid(schema, out validationErrors);

            int nullOccuranceCount = 0;
            var listOfErrors = FilterNullValueError(validationErrors, ref nullOccuranceCount);

            var errorGroup = listOfErrors.GroupBy(x => x.ErrorType);
            foreach (var grouping in errorGroup)
            {
                _logger.Error($"ErrorType = {grouping.Key}. Count = {grouping.Count()}");
            }

            var schemaValidationResult = new SchemaValidationResult
            {
                SchemaValidationErrors = listOfErrors,
                ParsedObjectsCount = parsedObjectsCount,
                NullOccurances = nullOccuranceCount
            };

            return schemaValidationResult;
        }

        private List<SchemaValidationError> FilterNullValueError(IList<ValidationError> validationErrors, ref int nullOccuranceCount)
        {
            var listOfErrors = new List<SchemaValidationError>();
            foreach (var error in validationErrors)
            {
                if (error.ErrorType == ErrorType.Type)
                {
                    if (error.Value == null)
                    {
                        nullOccuranceCount++;
                        /*_logger.Warning($"Encountered 'Null' value at line number {error.LineNumber} " +
                                        $"and position {error.LinePosition} for Property {error.Path}." +
                                        $"\nError Message: {error.Message}");*/
                        continue;
                    }
                }

                listOfErrors.Add(new SchemaValidationError()
                {
                    ErrorType = error.ErrorType,
                    Message = error.Message,
                    Path = error.Path,
                    Value = error.Value,
                    LineNumber = error.LineNumber,
                    LinePosition = error.LinePosition
                });
            }

            _logger.Information($"Number of 'Null' occurances is: {nullOccuranceCount}");

            return listOfErrors;
        }
    }
}