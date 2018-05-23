﻿using JsonValidationCoreWebApi.Validators;
using Moq;
using Newtonsoft.Json;
using Serilog;
using Xunit;

namespace JsonValidationCoreWebApi.UnitTests.Validators
{
    public class JsonValidatorShould
    {
        private readonly JsonValidator _jsonValidator;
        private Mock<ILogger> _loggerMock;

        public JsonValidatorShould()
        {
            _loggerMock = new Mock<ILogger>();
            _jsonValidator = new JsonValidator(_loggerMock.Object);
        }

        [Fact]
        public void Return_False_When_Json_Is_Empty()
        {

            var result = _jsonValidator.Validate("");

            Assert.False(result, "Json was incorrect. Validation reseult should be negative");
        }

        [Fact]
        public void Return_False_When_Json_Is_Null()
        {
            var result = _jsonValidator.Validate(null);

            Assert.False(result, "Json was incorrect. Validation reseult should be negative");
        }

        [Fact]
        public void Return_False_When_Json_Is_InValid()
        {
            var result = _jsonValidator.Validate(Constants.InvalidJson);
            Assert.False(result, "Json was incorrect. Validation reseult should be negative");
        }

        [Fact]
        public void Log_Error_When_Json_Is_InValid()
        {
            _jsonValidator.Validate(Constants.InvalidJson);
            _loggerMock.Verify(x => x.Error(It.IsAny<JsonReaderException>(), "Provided string is an invalid json"));
        }

        [Fact]
        public void Return_True_When_Json_Is_Valid()
        {
            var result = _jsonValidator.Validate(Constants.ValidJson);
            Assert.True(result, "Json was incorrect. Validation reseult should be positive");
        }

        [Fact]
        public void Return_True_When_Json_Contains_Special_Character()
        {
            var result = _jsonValidator.Validate(Constants.SpecialCharacterJson);
            Assert.True(result, "Json was incorrect. Validation reseult should be positive");
        }
    }
}