using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace JsonValidationCoreWebApi.AcceptanceTests.Schemas
{
    public class PropertyNotRequiredTypeSchema
    {
        [JsonProperty("web_pages", Required = Required.Always)]
        public List<string> WebPages { get; set; }

        [JsonProperty("name", Required = Required.AllowNull)]
        public string Name { get; set; }

        [JsonProperty("alpha_two_code", Required = Required.AllowNull)]
        public string AlphaTwoCode { get; set; }

        [JsonProperty("state_province", Required = Required.Default)]
        public string StateProvince { get; set; }

        [JsonProperty("domains", Required = Required.Always)]
        public List<string> Domains { get; set; }

        [JsonProperty("country", Required = Required.AllowNull)]
        public string Country { get; set; }
    }
}
