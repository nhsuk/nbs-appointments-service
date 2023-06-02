using System;
using Azure.Identity;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(NBS.Appointments.Service.Monitoring.Startup))]

namespace NBS.Appointments.Service.Monitoring
{
    class Startup : FunctionsStartup
    {
        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            string appConfigSetting = Environment.GetEnvironmentVariable("AppConfig");
            builder.ConfigurationBuilder.AddAzureAppConfiguration(options =>
            {
                options.Connect(appConfigSetting)
                        .ConfigureKeyVault(kv =>
                        {
                            kv.SetCredential(new DefaultAzureCredential());
                        });
            });
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddOptions<AzureAlertsHandler.Options>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection("Alerts").Bind(settings);
                });
        }
    }
}