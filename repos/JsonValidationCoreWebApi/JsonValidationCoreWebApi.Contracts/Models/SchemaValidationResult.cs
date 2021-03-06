﻿using System.Collections.Generic;

namespace JsonValidationCoreWebApi.Contracts.Models
{
    public class SchemaValidationResult
    {
        public List<SchemaValidationError> SchemaValidationErrors { get; set; }

        public int ParsedObjectsCount { get; set; }

        public int NullOccurances { get; set; }
    }
}
