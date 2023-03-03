using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;

namespace NBS.Appointments.Service.Extensions
{
    public static class ValidationErrorExtensions
    {
        public static IList<string> ToErrorMessages(this IList<ValidationFailure> failures)
        {
            return failures.Select(x => x.ErrorMessage).ToList();
        }
    }
}
