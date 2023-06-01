using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using Azure.Identity;
using Serilog;
using Serilog.Exceptions;
using NBS.Appointments.Service.Configuration;

namespace NBS.Appointments.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var settings = config.Build();
                    var appConfigSetting = settings.GetValue<string>("AppConfig", String.Empty);

                    if (String.IsNullOrEmpty(appConfigSetting) == false)
                    {
                        config.AddAzureAppConfiguration(options =>
                        {
                            options.Connect(appConfigSetting)
                                    .ConfigureKeyVault(kv =>
                                    {
                                        kv.SetCredential(new DefaultAzureCredential());
                                    });
                        });
                    }
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                {
                    var settings = hostingContext.Configuration;
                    var splunkConfig = settings.GetSection("Splunk").Get<SplunkSettings>();
                    loggerConfiguration
                        .WriteTo.EventCollector(splunkConfig.Host, splunkConfig.EventCollectorToken)
                        .Enrich.FromLogContext()
                        .Enrich.WithExceptionDetails();
                }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
