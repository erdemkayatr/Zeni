

namespace Zeni.Infra.Logging
{
    public static class ZeniLoggingConfigureMiddleware
    {
        public static IHostBuilder AddZeniLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {

            var config = new RabbitMQClientConfiguration
            {
                Port = int.Parse(configuration.GetSection("RabbitMQClientConfiguration:Port").Value),
                DeliveryMode = GetRabbitMQDeliveryMode(configuration.GetSection("RabbitMQClientConfiguration:DeliveryMode").Value),
                Exchange = configuration.GetSection("RabbitMQClientConfiguration:Exchange").Value,
                Username = configuration.GetSection("RabbitMQClientConfiguration:Username").Value,
                Password = configuration.GetSection("RabbitMQClientConfiguration:Password").Value,
                ExchangeType = configuration.GetSection("RabbitMQClientConfiguration:ExchangeType").Value,
                RouteKey = configuration.GetSection("RabbitMQClientConfiguration:RouteKey").Value
            };
            foreach (var item in configuration.GetSection("RabbitMQClientConfiguration:Hostnames").Get<IList<string>>())
            {
                config.Hostnames.Add(item);
            }

            var logLevel = GetLogEventLevel(configuration.GetSection("RabbitMQClientConfiguration:LogLevel").Value);
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext().MinimumLevel.Is(logLevel)
                .WriteTo.RabbitMQ((clientConfiguration, sinkConfiguration) =>
                {
                    clientConfiguration.From(config);
                    sinkConfiguration.TextFormatter = new JsonFormatter();
                }).CreateBootstrapLogger();

            hostBuilder.ConfigureLogging((hostingContext, lc) =>
            {
                lc.ClearProviders();
            }).UseSerilog(Log.Logger);
            return hostBuilder;
        }


        private static RabbitMQDeliveryMode GetRabbitMQDeliveryMode(string value)
        {
            if (value.Equals("RabbitMQDeliveryMode.NonDurable"))
            {
                return RabbitMQDeliveryMode.NonDurable;
            }
            else
            {
                return RabbitMQDeliveryMode.Durable;
            }

        }
        private static LogEventLevel GetLogEventLevel(string value)
        {
            switch (value)
            {
                case "Verbose": return LogEventLevel.Verbose;
                case "Debug": return LogEventLevel.Debug;
                case "Information": return LogEventLevel.Information;
                case "Warning": return LogEventLevel.Warning;
                case "Error": return LogEventLevel.Error;
                case "Fatal": return LogEventLevel.Fatal;
                default:
                    return LogEventLevel.Information;
            }
        }
    }
}
