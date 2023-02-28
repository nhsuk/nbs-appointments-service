using NBS.Appointments.Service.Core;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddQflowClient(
            this IServiceCollection services,
            string baseUrl,
            string userName,
            string password)
        {
            services.Configure<QflowOptions>(opts => {
                opts.BaseUrl = baseUrl;
                opts.UserName = userName;
                opts.Password = password;
            });
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
    }
}