using Newtonsoft.Json.Schema;

namespace JsonValidationCoreWebApi.Contracts.Models
{
    public class SchemaValidationError
    {
        public ErrorType ErrorType { get; set; }

        public string Message { get; set; }

        public string Path { get; set; }

        public int LineNumber { get; set; }

        public int LinePosition { get; set; }

        public object Value { get; set; }
    }
}
