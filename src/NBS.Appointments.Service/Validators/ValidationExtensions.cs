using FluentValidation;
using NBS.Appointments.Service.Core.Helpers;

namespace NBS.Appointments.Service.Validators
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeValidDescriptor<T, TDescriptor>(this IRuleBuilder<T, string> ruleBuilder) where TDescriptor : class, new()
        {
            return ruleBuilder.Must(MustBeValidDescriptor<TDescriptor>);
        }

        private static bool MustBeValidDescriptor<TDescriptor>(string descriptor) where TDescriptor : class, new()
        {
            try
            {
                DescriptorConverter.Parse<TDescriptor>(descriptor);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
