using System.Linq;
using JsonValidationCoreWebApi.Contracts.Models;
using JsonValidationCoreWebApi.HttpClients;
using JsonValidationCoreWebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        // POST api/schemavalidator
        [HttpPost]
        public IActionResult Post([FromBody] ValidateSchemaModel model)
        {
            var isValid = _jsonValidator.ValidateJson(model.Schema);
            if (!isValid)
            {
                var errorMessage = $"The given schema {model.Schema} is not a valid Json";
                _logger.Error(errorMessage);
                return BadRequest(new[] {errorMessage});
            }

            _logger.Information($"Calling Url: {model.Site} to get JSon data");
            var restApiResponse = _restApiClient.GetDataFromUrl(model.Site);
            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(model.Schema, restApiResponse.Data);

            var responseContent = JsonConvert.SerializeObject(schemaValidationResult);
            if (schemaValidationResult.SchemaValidationErrors.Any())
            {
                _logger.Error($"Validation Result: \n {responseContent}");
                return new ContentResult()
                {
                    StatusCode = 409,
                    Content = responseContent
                };
            }

            _logger.Information($"Validation Result: \n {responseContent}");
            return Ok(schemaValidationResult);
        }
    }
}