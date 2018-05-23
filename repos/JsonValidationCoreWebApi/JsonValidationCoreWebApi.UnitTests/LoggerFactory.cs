using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Xunit.Abstractions;

namespace JsonValidationCoreWebApi.UnitTests
{
    public static class LoggerFactory<T>
    {
        public static ILogger CreateLogger(ITestOutputHelper outputHelper)
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: Constants.MessageTemplate)
                .WriteTo.XunitTestOutput(outputHelper, outputTemplate: Constants.MessageTemplate)
                .CreateLogger()
                .ForContext<T>();
        }
    }
}
