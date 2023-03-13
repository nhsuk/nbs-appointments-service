using System;
using System.Linq;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NBS.Appointments.Service.Controllers;
using NBS.Appointments.Service.Validators;

namespace NBS.Appointments.Service
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterValidators(this IServiceCollection services)
        {
            Func<Type, bool> isValidatorInterface = t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IValidator<>);
            Func<Type, bool> isValidator = t => t.GetInterfaces().Any(isValidatorInterface);

            var converterTypes = typeof(RequestValidatorFactory).Assembly.GetTypes().Where(isValidator);

            foreach (var converterType in converterTypes)
            {
                var serviceType = converterType.GetInterfaces().Single(isValidatorInterface);
                services.AddSingleton(serviceType, converterType);
            }
            return services.AddSingleton<RequestValidatorFactory>();
        }
    }
}
