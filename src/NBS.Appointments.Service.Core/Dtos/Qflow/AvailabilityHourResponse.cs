namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class QflowAvailabilityHourResponse
    {
        public int ServiceId { get; set; }
        public int CalendarId { get; set; }
        public int AppointmentTypeId { get; set; }
        public int Duration { get; set; }
        public TimeSpan Time { get; set; }
    }
}
