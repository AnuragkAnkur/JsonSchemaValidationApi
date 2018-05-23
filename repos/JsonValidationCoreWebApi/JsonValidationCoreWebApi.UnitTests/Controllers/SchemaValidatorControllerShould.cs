using System.Reflection;
using JsonValidationCoreWebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace JsonValidationCoreWebApi.UnitTests.Controllers
{

    public class SchemaValidatorControllerShould
    {
        private readonly ILogger _logger;
        private readonly SchemaValidatorController _controller;
        private readonly Mock<IJsonValidator> _jsonValidatorMock;

        public SchemaValidatorControllerShould(ITestOutputHelper outputHelper)
        {
            _logger = LoggerFactory<SchemaValidatorControllerShould>.CreateLogger(outputHelper);
            _jsonValidatorMock = new Mock<IJsonValidator>();
            _controller = new SchemaValidatorController(_jsonValidatorMock.Object);
        }

        [Fact]
        public void Call_Json_Validator_To_Validate_Schema_Json()
        {
            var logger = _logger.ForContext("MemberName", MethodBase.GetCurrentMethod().Name);
            _jsonValidatorMock.Setup(x => x.Validate(Constants.ValidJson)).Returns(false);

            logger.Information($"Calling post method of {nameof(SchemaValidatorController)} contoller");
            _controller.Post(Constants.ValidJson);

            _jsonValidatorMock.Verify(x => x.Validate(Constants.ValidJson), Times.Once);
        }

        [Fact]
        public void Return_BadRequest_When_Schema_Json_Is_Invalid()
        {
            var logger = _logger.ForContext("MemberName", MethodBase.GetCurrentMethod().Name);
            _jsonValidatorMock.Setup(x => x.Validate(Constants.InvalidJson)).Returns(false);

            logger.Information($"Calling post method of {nameof(SchemaValidatorController)} contoller");
            var result = _controller.Post(Constants.ValidJson);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }
    }
}
