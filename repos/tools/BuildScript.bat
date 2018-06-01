dotnet build .\JsonValidationCoreWebApi
dotnet test .\JsonValidationCoreWebApi\JsonValidationCoreWebApi.UnitTests\JsonValidationCoreWebApi.UnitTests.csproj -r .\TestResults\
dotnet test .\JsonValidationCoreWebApi\JsonValidationCoreWebApi.IntegrationTests\JsonValidationCoreWebApi.IntegrationTests.csproj -r .\TestResults\
dotnet run --project .\JsonValidationCoreWebApi\JsonValidationCoreWebApi\JsonValidationCoreWebApi.csproj
