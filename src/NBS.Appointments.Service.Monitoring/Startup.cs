using System;
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
            string keyVaultUri = Environment.GetEnvironmentVariable("KeyVaultUri");
            builder.ConfigurationBuilder.AddAzureKeyVault(keyVaultUri);
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