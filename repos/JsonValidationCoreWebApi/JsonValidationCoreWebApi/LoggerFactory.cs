using Serilog;

namespace JsonValidationCoreWebApi
{
    public static class LoggerFactory
    {
        public const string MessageTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {SourceContext:l} :{MemberName} -> {Message}{NewLine}{Exception}";

        public static ILogger CreateLogger()
        {
            var logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("c:\\JsonValidationCoreWebApi-log-{Date}.txt"
                    , outputTemplate: MessageTemplate
                    , fileSizeLimitBytes:null)
                .WriteTo.Console(outputTemplate: MessageTemplate)
                .CreateLogger();

            return logger;
        }
    }
}
