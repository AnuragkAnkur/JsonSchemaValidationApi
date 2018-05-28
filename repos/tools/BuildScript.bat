".\tools\dotnet\dotnet.exe" build .\JsonValidationCoreWebApi
".\tools\dotnet\dotnet.exe" test .\JsonValidationCoreWebApi\JsonValidationCoreWebApi.UnitTests\JsonValidationCoreWebApi.UnitTests.csproj -r .\TestResults\
".\tools\dotnet\dotnet.exe" test .\JsonValidationCoreWebApi\JsonValidationCoreWebApi.IntegrationTests\JsonValidationCoreWebApi.IntegrationTests.csproj -r .\TestResults\
net stop "JsonWebApi"
".\tools\dotnet\dotnet.exe" publish .\JsonValidationCoreWebApi\JsonValidationCoreWebApi\JsonValidationCoreWebApi.csproj
sc create JsonWebApi binPath="%~dp0..\JsonValidationCoreWebApi\JsonValidationCoreWebApi\bin\Debug\netcoreapp20\win-x64\publish\JsonValidationCoreWebApi.exe"
sc start JsonWebApi
".\tools\dotnet\dotnet.exe" test .\JsonValidationCoreWebApi\JsonValidationCoreWebApi.AcceptanceTests\JsonValidationCoreWebApi.AcceptanceTests.csproj -r .\TestResults\