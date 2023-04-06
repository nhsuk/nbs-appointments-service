namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowCancelAppointmentDescriptor
    {
        public static QflowCancelAppointmentDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length != 3)
                throw new FormatException("Decscriptor is formatted incorrectly.");

            if (!int.TryParse(parts[1], out var qflowCustomerId))
                throw new FormatException("QflowCustomerId is in invalid format.");

            if (!int.TryParse(parts[2], out var processId))
                throw new FormatException("ProcessId must be a number.");

            return new QflowCancelAppointmentDescriptor(qflowCustomerId, processId);
        }

        public QflowCancelAppointmentDescriptor(int qflowCustomerId, int processId)
        {
            QflowCustomerId = qflowCustomerId;
            ProcessId = processId;
        }

        public int QflowCustomerId { get; set; }
        public int ProcessId { get; set; }
    }
}
