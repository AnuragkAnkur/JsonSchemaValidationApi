dotnet build ./JsonValidationCoreWebApi
dotnet test ./JsonValidationCoreWebApi/JsonValidationCoreWebApi.UnitTests/JsonValidationCoreWebApi.UnitTests.csproj
dotnet test ./JsonValidationCoreWebApi/JsonValidationCoreWebApi.IntegrationTests/JsonValidationCoreWebApi.IntegrationTests.csproj
dotnet run --project ./JsonValidationCoreWebApi/JsonValidationCoreWebApi/JsonValidationCoreWebApi.csproj &
dotnet test ./JsonValidationCoreWebApi/JsonValidationCoreWebApi.AcceptanceTests/JsonValidationCoreWebApi.AcceptanceTests.csproj