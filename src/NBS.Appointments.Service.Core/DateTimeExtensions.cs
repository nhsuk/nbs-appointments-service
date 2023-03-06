namespace NBS.Appointments.Service.Core
{
    public static class DateTimeExtensions
    {
        public static int DaysBetween(this DateTime start, DateTime end)
        {
            return ((int)(end-start).TotalDays);
        }
    }
}