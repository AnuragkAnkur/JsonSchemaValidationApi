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

            Assert.Equal(2, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal("name", schemaValidationResult.SchemaValidationErrors.First().Path);
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

            Assert.Equal(0, schemaValidationResult.ParsedObjectsCount);
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

            Assert.Equal(4, schemaValidationResult.ParsedObjectsCount);
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

            Assert.Equal(3, schemaValidationResult.ParsedObjectsCount);
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

            Assert.Equal(0, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MinimumItems, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_MaximumItems_Error_When_Empty_Json_Is_Validated_With_Required_Min_Size_Array_Schema()
        {
            var schema = @"{
                              'type': 'array',
                              'items' : {'type' : 'number'},
                              'maxItems' : 1
                            }";

            var json = @"[2,2,3]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(3, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MaximumItems, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
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

            Assert.Equal(4, schemaValidationResult.ParsedObjectsCount);
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
                    x.Warning(It.IsAny<string>()),Times.Once());
        }

        [Fact]
        public void Return_MaximumProperties_Error_When_Json_Contains_More_Than_One_Property()
        {
            var schema = @"{
                            'type': 'object',
                            'maxProperties' : 1
                            }";

            var json = @"{
                            'name' : 'foo',
                            'age' : '23'
                            }";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(2, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MaximumProperties, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_Maximum_Type_Error_When_Json_Contains_Greater_Than_Constrained_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'number', 'maximum': 0 }                            
                            }";

            var json = @"[1, 0, 1, 2]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(4, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(3, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.Maximum, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_Minimum_Type_Error_When_Json_Contains_Greater_Than_Constrained_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'number', 'minimum': 2 }                            
                            }";

            var json = @"[1, 0, 1, 2]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(4, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(3, schemaValidationResult.SchemaValidationErrors.Count);
            var result = schemaValidationResult.SchemaValidationErrors.All(x => x.ErrorType == ErrorType.Minimum);
            Assert.True(result);
        }

        [Fact]
        public void Return_MultipleOf_Type_Error_When_Json_Contains_Greater_Than_Constrained_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'number', 'multipleOf': 2 }                            
                            }";

            var json = @"[1, 0, 1, 2, 4, 5]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(6, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(3, schemaValidationResult.SchemaValidationErrors.Count);
            var result = schemaValidationResult.SchemaValidationErrors.All(x => x.ErrorType == ErrorType.MultipleOf);
            Assert.True(result);
        }

        [Fact]
        public void Return_UniqueItem_Type_Error_When_Json_Contains_Greater_Than_Constrained_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'number' },
                            'uniqueItems': true
                            }";

            var json = @"[1, 0, 1, 2, 4, 5]";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(6, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            var result = schemaValidationResult.SchemaValidationErrors.All(x => x.ErrorType == ErrorType.UniqueItems);
            Assert.True(result);
        }

        [Fact]
        public void Return_MaximumLength_Type_Error_When_Json_Contains_Greater_Than_Constrained_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'string', 'maxLength': 3 }                            
                            }";

            var json = @"['acb', 'dfgds', 'sdfssd', 'ab']";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(4, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(2, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MaximumLength, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_MinimumLength_Type_Error_When_Json_Contains_Smaller_Than_Constrained_Value()
        {
            var schema = @"{
                            'type': 'array',
                            'items': {'type': 'string', 'minLength': 3 }                            
                            }";

            var json = @"['acb', 'dfgds', 'sdfssd', 'ab']";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(4, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            Assert.Equal(ErrorType.MinimumLength, schemaValidationResult.SchemaValidationErrors.First().ErrorType);
        }

        [Fact]
        public void Return_Enum_Type_Error_When_Json_Contains_Value_Other_Than_Specified_Enum()
        {
            var schema = @"{
                            'type': 'object',
                            'properties': 
                                {
                                    'name': {'type':'string'},
                                    'MaritalStatus': {'type':'string', 'enum': ['married', 'unmarried','single']}
                                }                            
                            }";

            var json = @"{
                            'name': 'foo',
                             'MaritalStatus' : 'anything'
                        }";

            var schemaValidationResult = _jsonValidator.ValidateJsonAgainstSchema(schema, json);

            Assert.Equal(2, schemaValidationResult.ParsedObjectsCount);
            Assert.Equal(1, schemaValidationResult.SchemaValidationErrors.Count);
            var result = schemaValidationResult.SchemaValidationErrors.All(x => x.ErrorType == ErrorType.Enum);
            Assert.True(result);
        }
    }
}
