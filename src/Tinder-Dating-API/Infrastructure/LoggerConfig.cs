using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using System;

namespace Tinder_Dating_API.Infrastructure
{
    public static class LoggerConfig
    {
        public static void Configure()
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json", optional: false)
                            .Build();
                            

            var loggerOptions = new LoggerConfigOption();
            config.GetSection("LoggerConfigOption").Bind(loggerOptions);

            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.ControlledBy(new LoggingLevelSwitch(Serilog.Events.LogEventLevel.Debug))
                         .MinimumLevel.Override(loggerOptions.OverrideSource, Serilog.Events.LogEventLevel.Warning)
                         .WriteTo.Console(outputTemplate: loggerOptions.OutputTemplate)
                         .Enrich.FromLogContext()
                         .Enrich.WithProperty(nameof(Environment.MachineName), Environment.MachineName)
                         .CreateLogger();
        }
    }
}
