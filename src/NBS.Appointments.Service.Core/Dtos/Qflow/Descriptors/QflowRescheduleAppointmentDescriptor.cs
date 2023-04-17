namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowRescheduleAppointmentDescriptor
    {
        public static QflowRescheduleAppointmentDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length != 7)
                throw new FormatException("Descriptor was not formatted correctly.");

            if (parts[0] != "qflow")
                throw new FormatException("Descriptor was not a valid qflow appointment descriptor.");

            if (!int.TryParse(parts[1], out var serviceId))
                throw new FormatException("ServiceId must be a number.");

            if (!int.TryParse(parts[2], out var originalProcessId))
                throw new FormatException("OriginalProcessId must be a number.");

            if (!int.TryParse(parts[3], out var appointmentTypeId))
                throw new FormatException("AppointmentTypeId must be a number.");

            if (!DateTime.TryParse(parts[4], out var appointmentDate))
                throw new FormatException("Appointment date must be a valid date.");

            if (!int.TryParse(parts[5], out var time))
                throw new FormatException("Start time must be a number.");

            if (!int.TryParse(parts[6], out var cancelationReasonId))
                throw new FormatException("CancelationReasonId must be a number.");

            var timeSpan = TimeSpan.FromMinutes(time);
            appointmentDate = appointmentDate.Add(timeSpan);

            return new QflowRescheduleAppointmentDescriptor(
                serviceId,
                appointmentDate,
                appointmentTypeId,
                cancelationReasonId,
                originalProcessId);
        }

        public QflowRescheduleAppointmentDescriptor(int serviceId, DateTime dateAndTime, int appointmentTypeId, int cancelationReasonId, long originalProcessId)
        {
            ServiceId = serviceId;
            DateAndTime = dateAndTime;
            AppointmentTypeId = appointmentTypeId;
            CancelationReasonId = cancelationReasonId;
            OriginalProcessId = originalProcessId;
        }

        public int ServiceId { get; set; }
        public DateTime DateAndTime { get; set; }
        public int AppointmentTypeId { get; set; }
        public int CancelationReasonId { get; set; }
        public long OriginalProcessId { get; set; }
    }
}
