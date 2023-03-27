namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowAppointmentDescriptor
    {
        public static QflowAppointmentDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(":");

            if (parts.Length != 5)
                throw new FormatException("Descriptor is not formatted correctly");

            if (parts[0] != "qflow")
                throw new FormatException("String was not a qflow service descriptor");

            if (!int.TryParse(parts[1], out var slotOrdinalNumber))
                throw new FormatException("Slot ordinal number must be a number.");

            if (!int.TryParse(parts[2], out var serviceId))
                throw new FormatException("ServiceId must be a number.");

            if (!DateTime.TryParse(parts[3], out var appointmentDateAndTime))
                throw new FormatException("Appointment date and time must be a valid date and time.");

            if (!int.TryParse(parts[5], out var calendarId))
                throw new FormatException("CalendarId must be a number.");

            return new QflowAppointmentDescriptor(
                slotOrdinalNumber,
                serviceId,
                appointmentDateAndTime,
                parts[4],
                calendarId);
        }

        public QflowAppointmentDescriptor(
            int slotOrdinalNumber,
            int serviceId,
            DateTime dateAndTime,
            string service,
            int calendarId)
        {
            SlotOrdinalNumber = slotOrdinalNumber;
            ServiceId = serviceId;
            DateAndTime = dateAndTime;
            Service = service;
            CalendarId = calendarId;
        }

        public int SlotOrdinalNumber { get; set; }
        public int ServiceId { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Service { get; set; }
        public int CalendarId { get; set; }
    }
}
