namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowBookAppointmentDescriptor
    {
        public static QflowBookAppointmentDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(":");

            if (parts.Length != 7)
                throw new FormatException("Descriptor is not formatted correctly");

            if (parts[0] != "qflow")
                throw new FormatException("String was not a qflow service descriptor");

            if (!int.TryParse(parts[1], out var serviceId))
                throw new FormatException("ServiceId must be a number.");

            if (!int.TryParse(parts[2], out var calendarId))
                throw new FormatException("CalendarId must be a number.");

            if (!int.TryParse(parts[3], out var appointmentTypeId))
                throw new FormatException("AppointmentTypeId must be a number.");

            if (!DateTime.TryParse(parts[4], out var appointmentDate))
                throw new FormatException("Appointment date must be a valid date.");

            if (!int.TryParse(parts[5], out var appointmentTime))
                throw new FormatException("Appointment time must be a number.");

            if (!int.TryParse(parts[6], out var slotOrdinalNumber))
                throw new FormatException("Slot ordinal number must be a number.");

            var timespan = TimeSpan.FromMinutes(appointmentTime);
            appointmentDate = appointmentDate.Add(timespan);

            return new QflowBookAppointmentDescriptor(
                serviceId,
                calendarId,
                appointmentTypeId,
                appointmentDate,
                slotOrdinalNumber);
        }

        public QflowBookAppointmentDescriptor(
            int serviceId,
            int calendarId,
            int appointmentTypeId,
            DateTime appointmentDateAndTime,
            int slotOrdinalNumber)
        {
            ServiceId = serviceId;
            CalendarId = calendarId;
            AppointmentTypeId = appointmentTypeId;
            AppointmentDateAndTime = appointmentDateAndTime;
            SlotOrdinalNumber = slotOrdinalNumber;
        }

        public int ServiceId { get; set; }
        public int CalendarId { get; set; }
        public int AppointmentTypeId { get; set; }
        public DateTime AppointmentDateAndTime { get; set; }
        public int SlotOrdinalNumber { get; set; }
    }
}
