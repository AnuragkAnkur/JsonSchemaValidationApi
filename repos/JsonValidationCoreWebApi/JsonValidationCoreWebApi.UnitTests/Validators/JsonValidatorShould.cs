using System.Linq;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Serilog;
using Xunit;
using JsonValidator = JsonValidationCoreWebApi.Validators.JsonValidator;

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

        [Fact]
        public void Return_Type_Error_When_Property_Type_Is_Integer_And_Value_Is_String()
        {
            var schema = @"{
                              'type': 'object',
                              'properties': {
                                'name': {'type':'integer'},
                                'roles': {'type': 'array'}
                              }
                            }";

            var json = @"{
                              'name': 'Arnie Admin',
                              'roles': ['Developer', 'Administrator']
                            }";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(0, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal("Arnie Admin", schemaValidationResult.SchemaValidationErrors.First().Value);
            Assert.Equal(ErrorType.Type, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_Required_Error_When_Empty_Json_Is_Validated_Against_An_Required_Object_Schema()
        {
            var schema = @"{
                              'type': 'object',
                              'properties': {
                                'name': {'type':'integer'},
                                'roles': {'type': 'array'}
                              },
                              'required': ['name']
                            }";

            var json = @"{}";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(0, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Contains("Required properties are missing from object: name.",
                schemaValidationResult.SchemaValidationErrors.First().Message);

            Assert.Equal(ErrorType.Required, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_No_Error_When_A_Valid_Json_Matching_Schema()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type':'string'}
                            }";
            
            var json = @"['Chilean', 'Argentinean', 'Peruvian', 'Colombian']";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(4, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.False(schemaValidationResult.SchemaValidationErrors.Any());
        }

        [Fact]
        public void Return_AdditionalProperties_Error_When_Json_Contains_More_Properties_Than_Schema()
        {
            var schema = @"{
                              'type': 'object',
                              'properties': {
                                'name': {'type':'string'},
                                'roles': {'type': 'array'}
                              },
                            'additionalProperties' : false
                            }";

            var json = @"{
                              'name': 'Arnie Admin',
                              'id'  : 'bond',
                              'roles': ['Developer', 'Administrator']
                            }";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(0, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.AdditionalProperties, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_MinimumItems_Error_When_Empty_Json_Is_Validated_With_Required_Min_Size_Array_Schema()
        {
            var schema = @"{
                              'type': 'array',
                              'items' : {'type' : 'string'},
                              'minItems' : 1
                            }";

            var json = @"[]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(0, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MinimumItems, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_No_Error_When_Json_Properties_Contain_Null_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'number'}
                            }";

            var json = @"[1, null, 1, 2]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(4, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.False(schemaValidationResult.SchemaValidationErrors.Any());
        }

        [Fact]
        public void Print_Encoutered_Null_Occurances()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'number'}
                            }";

            var json = @"[1, null, 1, 2]";

            _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            _loggerMock.Verify(x => 
                    x.Warning("Encountered 'Null' value at line number 1and position 8." +
                                              "Error Message: Invalid type. Expected Number but got Null."),
            Times.Once());
        }

        [Fact]
        public void Return_MinimumProperties_Error_When_Json_Is_Empty()
        {
            var schema = @"{
                            'type': 'object',
                            'minProperties' : 1
                            }";

            var json = @"{}";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(0, schemaValidationResult.SuccessfullyParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MinimumProperties, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }
    }
}
