using Azure.Identity;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core;
using NBS.Appointments.Service.Core.Helpers;
using NBS.Appointments.Service.Core.Interfaces;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddQflowClient(
            this IServiceCollection services)
        {            
            return services
                .AddSingleton<IQflowSessionManager, QflowSessionManager>()
                .AddTransient<IQflowService, QflowService>();
        }

        public static IServiceCollection AddSessionManager(this IServiceCollection services, SessionManagerOptions options)
        {
            if (options.Type == SessionManagerOptions.AzureStorage)
                return services.AddAzureBlobStoreMutex(options.BlobEndpoint, options.ContainerName);
            else if (options.Type == SessionManagerOptions.InMemory)
                return services.AddInMemoryStoreMutex();

            throw new ArgumentOutOfRangeException($"No session manage of type {options.Type} is available");
        }

        public static IServiceCollection AddAzureBlobStoreMutex(this IServiceCollection services, string blobEndpoint, string containerName)
        {
            services.Configure<SessionManagerOptions>(opts => {
                opts.Type = SessionManagerOptions.AzureStorage;
                opts.ContainerName = containerName;
            });
            services.AddAzureClients(x =>
            {
                x.AddBlobServiceClient(new Uri(blobEndpoint));
                x.UseCredential(new DefaultAzureCredential());
            });
            return services.AddSingleton<IMutexRecordStore, AzureBlobMutexRecordStore>();
        }

        public static IServiceCollection AddInMemoryStoreMutex(this IServiceCollection services)
        {
            return services.AddSingleton<IMutexRecordStore, InMemoryMutexRecordStore>();
        }

        public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
        {
            var opts = services.BuildServiceProvider().GetRequiredService<IOptions<DateTimeProviderOptions>>();
            
            switch (opts.Value.Type.ToLower())
            {
                case "system":
                    return services.AddTransient<IDateTimeProvider, SystemDateTimeProvider>();
                case "remote":
                    return services.AddSingleton<IDateTimeProvider, RemoteDateTimeProvider>();
                default:
                    throw new NotSupportedException("Unsupported date time proivder");
            }            
        }

        public static IServiceCollection AddHelpers(this IServiceCollection services)
        {
            return services.AddSingleton<ICustomPropertiesHelper, CustomPropertiesHelper>();
        }
    }
}
