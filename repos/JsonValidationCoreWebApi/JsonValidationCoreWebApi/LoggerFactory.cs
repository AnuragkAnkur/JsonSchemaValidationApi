using Serilog;

namespace JsonValidationCoreWebApi
{
    public static class LoggerFactory
    {
        public const string MessageTemplate =
            "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level}] {SourceContext:l} :{MemberName} -> {Message}{NewLine}{Exception}";

        public static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(".\\bin\\debug\\Logs\\JsonValidationCoreWebApi-log-{Date}.txt", outputTemplate: MessageTemplate)
                .WriteTo.Console(outputTemplate: MessageTemplate)
                .CreateLogger();
        }
    }
}
