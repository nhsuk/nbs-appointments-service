using NBS.Appointments.Service.Core;
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

        public static IServiceCollection AddAzureBlobStoreMutex(this IServiceCollection services, string containerName)
        {
            services.Configure<AzureBlobMutexRecordStore.Options>(opts => {
                opts.ContainerName = containerName;
            });
            return services.AddSingleton<IMutexRecordStore, AzureBlobMutexRecordStore>();
        }

        public static IServiceCollection AddInMemoryStoreMutex(this IServiceCollection services)
        {
            return services.AddSingleton<IMutexRecordStore, InMemoryMutexRecordStore>();
        }

        public static IServiceCollection AddDateTimeProvider(this IServiceCollection services, string configString)
        {
            var config = configString.Split("@");
            switch (config[0].ToLower())
            {
                case "system":
                    return services.AddTransient<IDateTimeProvider, SystemDateTimeProvider>();
                case "remote":
                    services.Configure<RemoteDateTimeProviderOptions>(opts => opts.Endpoint = config[1]);
                    return services.AddSingleton<IDateTimeProvider, RemoteDateTimeProvider>();
                default:
                    throw new NotSupportedException("Unsupported date time proivder");
            }            
        }
    }
}
