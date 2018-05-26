using System.Collections.Generic;

namespace JsonValidationCoreWebApi.Models
{
    public class SchemaValidationResult
    {
        public IList<SchemaValidationError> SchemaValidationErrors { get; set; }

        public int SuccessfullyParsedObjectsCount { get; set; }
    }
}
