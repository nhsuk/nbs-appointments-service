using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace NBS.Appointments.Service.Validators
{
    public class RequestValidatorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public RequestValidatorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IValidator<T> GetValidator<T>()
        {
            return _serviceProvider.GetRequiredService(typeof(IValidator<T>)) as IValidator<T>;
        }
    }
}
