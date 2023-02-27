using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Core.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddQflowClient(this IServiceCollection services)
        {
            return services.AddTransient<IQflowService, QflowService>();            
        }
    }
}