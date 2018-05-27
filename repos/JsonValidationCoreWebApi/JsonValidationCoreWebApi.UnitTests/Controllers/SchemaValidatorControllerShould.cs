using System.Collections.Generic;
using System.Net;
using JsonValidationCoreWebApi.Contracts.Models;
using JsonValidationCoreWebApi.Controllers;
using JsonValidationCoreWebApi.HttpClients;
using JsonValidationCoreWebApi.Validators;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Schema;
using Serilog;
using Xunit;

namespace JsonValidationCoreWebApi.UnitTests.Controllers
{

    public class SchemaValidatorControllerShould
    {
        private readonly SchemaValidatorController _controller;
        private readonly Mock<IJsonValidator> _jsonValidatorMock;
        private readonly Mock<ILogger> _loggerMock;
        private readonly Mock<IRestApiClient> _restApiClientMock;
        private readonly string _apiEndpoint;
        private readonly RestApiResponse _restApiResponse;

        public SchemaValidatorControllerShould()
        {
            _jsonValidatorMock = new Mock<IJsonValidator>();
            _loggerMock = new Mock<ILogger>();
            _apiEndpoint = "http://localhost:7675";
            _restApiResponse = new RestApiResponse()
            {
                Data = string.Empty
            };

            _restApiClientMock = new Mock<IRestApiClient>();
            _restApiClientMock.Setup(x => x.GetDataFromUrl(_apiEndpoint)).Returns(_restApiResponse);
            _controller = new SchemaValidatorController(_jsonValidatorMock.Object, _loggerMock.Object, _restApiClientMock.Object);
        }

        [Fact]
        public void Call_Json_Validator_To_Validate_Schema_Json()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.ValidJson)).Returns(false);

            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.ValidJson,
                Site = _apiEndpoint
            };

            _controller.Post(validateSchemaModel);

            _jsonValidatorMock.Verify(x => x.ValidateJson(Constants.ValidJson), Times.Once);
        }

        [Fact]
        public void Return_BadRequest_When_Schema_Json_Is_Invalid()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.InvalidJson)).Returns(false);

            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.ValidJson,
                Site = _apiEndpoint
            };

            var result = _controller.Post(validateSchemaModel);

            Assert.IsAssignableFrom<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Log_Error_When_Schema_Validation_Fails()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.InvalidJson)).Returns(false);

            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.InvalidJson,
                Site = _apiEndpoint
            };

            _controller.Post(validateSchemaModel);

            _loggerMock.Verify(x => x.Error($"The given schema {Constants.InvalidJson} is not a valid Json"));
        }

        [Fact]
        public void Return_OK_When_Schema_Json_Is_Valid()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.ValidJson)).Returns(true);
            _jsonValidatorMock.Setup(x => x.ValidateJsonAgainstSchema(Constants.ValidJson, _restApiResponse.Data))
                .Returns(
                    new SchemaValidationResult()
                    {
                        SuccessfullyParsedObjectsCount = 1,
                        SchemaValidationErrors = new List<SchemaValidationError>()
                    }
                );

            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.ValidJson,
                Site = _apiEndpoint
            };

            var result =  _controller.Post(validateSchemaModel);

            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void Return_Conflict_When_Schema_Validation_Fails()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.ValidJson)).Returns(true);
            _jsonValidatorMock.Setup(x => x.ValidateJsonAgainstSchema(Constants.ValidJson, _restApiResponse.Data))
                .Returns(
                    new SchemaValidationResult()
                    {
                        SuccessfullyParsedObjectsCount = 0,
                        SchemaValidationErrors = new List<SchemaValidationError>()
                        {
                            new SchemaValidationError()
                            {
                                ErrorType = ErrorType.Type,
                                Message = "Test Error",
                                Path = null
                            }
                        }
                    }
                );

            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.ValidJson,
                Site = _apiEndpoint
            };

            var result = _controller.Post(validateSchemaModel);

            Assert.IsAssignableFrom<ContentResult>(result);
            var contentResult = (ContentResult) result;
            Assert.Equal((int)HttpStatusCode.Conflict , contentResult.StatusCode.Value);
        }

        [Fact]
        public void Call_Rest_Api_Client_To_Get_Data_From_A_Given_Api_Endpoint()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.ValidJson)).Returns(true);
            _jsonValidatorMock.Setup(x => x.ValidateJsonAgainstSchema(Constants.ValidJson, _restApiResponse.Data))
                .Returns(
                    new SchemaValidationResult()
                    {
                        SuccessfullyParsedObjectsCount = 1,
                        SchemaValidationErrors = new List<SchemaValidationError>()
                    }
                );


            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.ValidJson,
                Site = _apiEndpoint
            };

            _controller.Post(validateSchemaModel);

            _restApiClientMock.Verify(x => x.GetDataFromUrl(validateSchemaModel.Site), Times.Once);
        }

        [Fact]
        public void Validate_Response_Returned_By_A_Rest_Api_Against_A_Given_Json_Schema()
        {
            _jsonValidatorMock.Setup(x => x.ValidateJson(Constants.ValidJson)).Returns(true);
            _jsonValidatorMock.Setup(x => x.ValidateJsonAgainstSchema(Constants.ValidJson, _restApiResponse.Data))
                .Returns(
                    new SchemaValidationResult()
                    {
                        SuccessfullyParsedObjectsCount = 1,
                        SchemaValidationErrors = new List<SchemaValidationError>()
                    }
                );

            var validateSchemaModel = new ValidateSchemaModel()
            {
                Schema = Constants.ValidJson,
                Site = _apiEndpoint
            };

            _controller.Post(validateSchemaModel);

            _jsonValidatorMock.Verify(x => x.ValidateJsonAgainstSchema(validateSchemaModel.Schema, _restApiResponse.Data), Times.Once);
        }
    }
}
