using System.Collections.Generic;
using JsonValidationCoreWebApi.Validators;
using Microsoft.AspNetCore.Mvc;

namespace JsonValidationCoreWebApi.Controllers
{
    [Route("api/[controller]")]
    public class SchemaValidatorController : Controller
    {
        private readonly IJsonValidator _jsonValidator;

        public SchemaValidatorController(IJsonValidator jsonValidator)
        {
            _jsonValidator = jsonValidator;
        }

        // GET api/schemavalidator
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/schemavalidator/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/schemavalidator
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            var isValid = _jsonValidator.Validate(value);
            if (!isValid)
            {
                var errorMessage = "Json schema is not a valid Json";
                var errors = new string[] {errorMessage};
                return BadRequest(errors);
            }

            return Ok("Schema validation has passed");
        }

        // PUT api/schemavalidator/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
