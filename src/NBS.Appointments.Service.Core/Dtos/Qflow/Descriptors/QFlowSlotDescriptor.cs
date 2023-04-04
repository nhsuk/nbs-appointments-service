namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QFlowSlotDescriptor
    {
        public static QFlowSlotDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length != 7)
                throw new FormatException("Slot descriptor is not formatted correctly.");

            if (parts[0] != "qflow")
                throw new FormatException("String was not a qflow slot descriptor.");

            if (!int.TryParse(parts[1], out var serviceId))
                throw new FormatException("ServiceId must be a number.");

            if (!int.TryParse(parts[2], out var calendarId))
                throw new FormatException("CalendarId must be a number.");

            if (!int.TryParse(parts[3], out var appointmentTypeId))
                throw new FormatException("Appointment type must be a number.");

            if (!DateTime.TryParse(parts[4], out var date))
                throw new FormatException("Date must be a valid date.");

            if (!int.TryParse(parts[5], out var startTime))
                throw new FormatException("Start time must be a number.");

            if (!int.TryParse(parts[6], out var endTime))
                throw new FormatException("End time must be a number.");

            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time.");

            return new QFlowSlotDescriptor(serviceId, calendarId, appointmentTypeId, date, startTime, endTime);
        }

        public QFlowSlotDescriptor(int serviceId, int calendarId, int appointmentTypeId, DateTime date, int startTime, int endTime)
        {
            ServiceId = serviceId;
            CalendarId = calendarId;
            AppointmentTypeId = appointmentTypeId;
            Date = date;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int ServiceId { get; set; }
        public int CalendarId { get; set; }
        public int AppointmentTypeId { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
