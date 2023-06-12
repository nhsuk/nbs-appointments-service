using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
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
                    var keyVaultUri = settings.GetValue<string>("KeyVaultUri", string.Empty);

                    if (String.IsNullOrEmpty(keyVaultUri) == false)
                    {
                        config.AddAzureKeyVault(keyVaultUri);
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
