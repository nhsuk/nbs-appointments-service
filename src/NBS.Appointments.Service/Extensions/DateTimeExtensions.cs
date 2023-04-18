using System.Linq;
using System;

namespace NBS.Appointments.Service.Extensions
{
    public static class DateTimeExtensions
    {
        private const char PlusChar = '+';
        private const char MinusChar = '-';

        public static DateTime UtcFromTimezoneOffset(this DateTime appointmentTime, string timezoneOffset)
        {
            const string noOffset = "0";
            if (string.IsNullOrEmpty(timezoneOffset))
                timezoneOffset = noOffset;

            // Get the timezone offset number as a string then parse to integer
            var numberString = new string(timezoneOffset.Where(char.IsDigit).ToArray());
            var offset = int.Parse(numberString);

            var isPlus = timezoneOffset.Contains(PlusChar);
            var isMinus = timezoneOffset.Contains(MinusChar);

            // No offset so return original time
            if (!isPlus && !isMinus)
                return appointmentTime;

            return isPlus
                ? appointmentTime.AddHours(-offset)
                : appointmentTime.AddHours(offset);
        }
    }
}
