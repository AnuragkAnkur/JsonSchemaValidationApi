using System.Reflection;
using JsonValidationCoreWebApi.Controllers;
using JsonValidationCoreWebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Serilog;
using Xunit;
using Xunit.Abstractions;

namespace JsonValidationCoreWebApi.UnitTests.Controllers
{

    public class SchemaValidatorControllerShould
    {
        private readonly SchemaValidatorController _controller;
        private readonly Mock<IJsonValidator> _jsonValidatorMock;
        private Mock<ILogger> _loggerMock;

        public SchemaValidatorControllerShould(ITestOutputHelper outputHelper)
        {
            _jsonValidatorMock = new Mock<IJsonValidator>();
            _loggerMock = new Mock<ILogger>();
            _controller = new SchemaValidatorController(_jsonValidatorMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Call_Json_Validator_To_Validate_Schema_Json()
        {
            _jsonValidatorMock.Setup(x => x.Validate(Constants.ValidJson)).Returns(false);

            _controller.Post(Constants.ValidJson);

            _jsonValidatorMock.Verify(x => x.Validate(Constants.ValidJson), Times.Once);
        }

        [Fact]
        public void Return_BadRequest_When_Schema_Json_Is_Invalid()
        {
            _jsonValidatorMock.Setup(x => x.Validate(Constants.InvalidJson)).Returns(false);

            var result = _controller.Post(Constants.InvalidJson);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Log_Error_When_Schema_Validation_Fails()
        {
            _jsonValidatorMock.Setup(x => x.Validate(Constants.InvalidJson)).Returns(false);

            _controller.Post(Constants.InvalidJson);

            _loggerMock.Verify(x => x.Error($"The given schema {Constants.InvalidJson} is not a valid Json"));
        }
    }
}
