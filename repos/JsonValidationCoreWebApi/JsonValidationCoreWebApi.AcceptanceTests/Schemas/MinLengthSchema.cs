using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Newtonsoft.Json;

namespace JsonValidationCoreWebApi.AcceptanceTests.Schemas
{
    public class MinLengthSchema
    {
        [JsonProperty("web_pages", Required = Required.Always)]
        [MinLength(2)]
        public List<string> WebPages { get; set; }

        [JsonProperty("name", Required = Required.AllowNull)]
        public string Name { get; set; }

        [JsonProperty("alpha_two_code", Required = Required.AllowNull)]

        public string AlphaTwoCode { get; set; }

        [JsonProperty("state-province", Required = Required.AllowNull)]
        public string StateProvince { get; set; }

        [JsonProperty("domains", Required = Required.Always)]
        [MinLength(1)]
        public List<string> Domains { get; set; }

        [JsonProperty("country", Required = Required.Default)]
        public string Country { get; set; }
    }
}
