using JsonValidationCoreWebApi.Validators;
using Moq;
using Newtonsoft.Json;
using Serilog;
using Xunit;

namespace JsonValidationCoreWebApi.UnitTests.Validators
{
    public class JsonValidatorShould
    {
        private readonly JsonValidator _jsonValidator;
        private readonly Mock<ILogger> _loggerMock;

        public JsonValidatorShould()
        {
            _loggerMock = new Mock<ILogger>();
            var loggerMockObject = _loggerMock.Object;
            _loggerMock.Setup(x => x.ForContext<JsonValidator>())
                .Returns(loggerMockObject);

            _jsonValidator = new JsonValidator(loggerMockObject);
        }

        [Fact]
        public void Return_False_When_Json_Is_Empty()
        {

            var result = _jsonValidator.ValidateJson("");

            Assert.False(result, "Json was incorrect. Validation reseult should be negative");
        }

        [Fact]
        public void Return_False_When_Json_Is_Null()
        {
            var result = _jsonValidator.ValidateJson(null);

            Assert.False(result, "Json was incorrect. Validation reseult should be negative");
        }

        [Fact]
        public void Return_False_When_Json_Is_InValid()
        {
            var result = _jsonValidator.ValidateJson(Constants.InvalidJson);
            Assert.False(result, "Json was incorrect. Validation reseult should be negative");
        }

        [Fact]
        public void Log_Error_When_Json_Is_InValid()
        {
            _jsonValidator.ValidateJson(Constants.InvalidJson);
            _loggerMock.Verify(x => x.Error(It.IsAny<JsonReaderException>(), "Provided string is an invalid json"));
        }

        [Fact]
        public void Return_True_When_Json_Is_Valid()
        {
            var result = _jsonValidator.ValidateJson(Constants.ValidJson);
            Assert.True(result, "Json was incorrect. Validation reseult should be positive");
        }

        [Fact]
        public void Return_True_When_Json_Contains_Special_Character()
        {
            var result = _jsonValidator.ValidateJson(Constants.SpecialCharacterJson);
            Assert.True(result, "Json was incorrect. Validation reseult should be positive");
        }
    }
}
