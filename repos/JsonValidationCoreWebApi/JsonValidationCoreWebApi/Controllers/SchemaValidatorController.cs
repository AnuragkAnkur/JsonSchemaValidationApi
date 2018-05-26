using System.Collections.Generic;
using System.Linq;
using JsonValidationCoreWebApi.HttpClients;
using JsonValidationCoreWebApi.Models;
using JsonValidationCoreWebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace JsonValidationCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    public class SchemaValidatorController : Controller
    {
        private readonly IJsonValidator _jsonValidator;
        private readonly ILogger _logger;
        private readonly IRestApiClient _restApiClient;

        public SchemaValidatorController(IJsonValidator jsonValidator, ILogger logger, IRestApiClient restApiClient)
        {
            _jsonValidator = jsonValidator;
            _logger = logger;
            _restApiClient = restApiClient;
        }

        // GET api/schemavalidator
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] {"value1", "value2"};
        }

        // POST api/schemavalidator
        [HttpPost]
        public IActionResult Post([FromBody] ValidateSchemaModel model)
        {
            var isValid = _jsonValidator.ValidateJson(model.Schema);
            if (!isValid)
            {
                var errorMessage = $"The given schema {model.Schema} is not a valid Json";
                var errors = new string[] {errorMessage};
                _logger.Error(errorMessage);
                return BadRequest(errors);
            }

            var restApiResponse = _restApiClient.GetDataFromUrl(model.Site);
            var validationErrors = _jsonValidator.ValidateJsonAgainstSchema(model.Schema, restApiResponse.Data);
            return Ok("Schema validation has passed");
        }
    }
}
