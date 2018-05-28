# **Project Synopsis**
    
        **Name**: JsonValidationCoreWebApi
        **Summary**: It is web API which is build on dot-net core framework. Programming language used is c#
        The API is responsible to validate Json which is fetched from a website against a supplied Json Schema.
        
        The web API only supports POST operation which take a schema and a web site URL as input body and returns a validation result
        with number of object parsed along with list of errors encountered after validating Json against the schema. 

# **Solution Structure**
The solution consists of five projects namely:

    1. JsonValidationCoreWebApi - The dot-net core web api.
    2. JsonValidationCoreWebApi.Contracts - Contains models and         contracts.
    3. JsonValidationCoreWebApi.UnitTests - Contains unit tests related     to classes in JsonValidationCoreWebApi project.
    4. JsonValidationCoreWebApi.IntegrationTests- Contains integration test for the RestApiClient.cs
    5. JsonValidationCoreWebApi.AcceptanceTests - Constains acceptance tests. 
        Dependency- The acceptance tests should run against the deployed service.So run the BuildScript.cmd to run the accceptance tests.


# **Main Parts**
The web API has got three main classes each responsible for the following purposes:

    1. Controller: SchemaValidatorController.cs
        To interact with HTTP Request and response.
    2. RestApiClient.cs: 
        To call the website given in the request body and get Json data for validation.
    3. JsonValidator.cs
        To validate the Json data against the schema provided in the web request body.
        The validator treats 'Null' as a default value. Rest any error is considered as error and becomes part of the validation result.
        Any 'Null' value encountered is logged as information. 
        Validator also counts the number of object parsed in a Json and adds it the Validation result.
        
# **User References:**
    1. Most of the test coverage around JsonValidator.cs is written as unit test. This is because it is fast and easy.
    2. There are 34 different types of JsonSchema validation errors. The unit test coverage is not around all of them but around 14 such errors are covered using unit testing. 
    3. The webapi is hosted as a windows service and can be run using the provided build script.
    4. To run the dot-net core application, dotnet tool is provided with the solution under .\repo\tools\dotnet folder.
    5. BuildScript.cmd is located at .\repo\tools\BuildScript.cmd

# **Steps to run BuildScript.cmd**
    1. Open command prompt in admin mode because it creates service
    2. cd to .\repo directory
    3. Run .\tools\BuildScript.cmd
 **This build script will** 
 
     1.Compile the solution.
     2. Run the unit tests
     3. Run the integration tests
     4. Publish the package
     5. Install the web api as a windows service
     6. Start the service
     7. Run the acceptance tests.


    
# **Web API Request Example**
    Endpoint: http://localhost:5000/api/schemavalidator
    Body :
        `{
        	"Schema": {
        				"type": "array",
        				"items": {"type": "number"}
                       },
        	"Site": "http://givemejsondata.com"
        }`
    Method: POST
    Content-Type: application/json
