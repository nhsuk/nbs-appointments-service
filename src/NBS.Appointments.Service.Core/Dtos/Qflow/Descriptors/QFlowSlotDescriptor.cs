namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QFlowSlotDescriptor
    {
        public static QFlowSlotDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length != 4)
                throw new FormatException("Slot descriptor is not formatted correctly.");

            if (parts[0] != "qflow")
                throw new FormatException("String was not a qflow slot descriptor.");

            if (!int.TryParse(parts[1], out var calendarId))
                throw new FormatException("CalendarId in invalid format.");

            if (!int.TryParse(parts[2], out var startTime))
                throw new FormatException("Start time is in invalid format.");

            if (!int.TryParse(parts[3], out var endTime))
                throw new FormatException("End time is in invalid format.");

            if (startTime >= endTime)
                throw new ArgumentException("Start time must be before end time.");

            return new QFlowSlotDescriptor(calendarId, startTime, endTime);
        }

        public QFlowSlotDescriptor(int calendarId, int startTime, int endTime)
        {
            CalendarId = calendarId;
            StartTime = startTime;
            EndTime = endTime;
        }

        public int CalendarId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
