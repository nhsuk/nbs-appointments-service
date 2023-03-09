using Microsoft.Extensions.DependencyInjection;
using NBS.Appointments.Service.Validators;

namespace NBS.Appointments.Service
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            services
                .AddTransient<AvailabilityByHourRequestValidator>()
                .AddTransient<ReserveSlotRequestVadidator>();

            return services;
        }
    }
}
