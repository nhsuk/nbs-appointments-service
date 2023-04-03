namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowCustomPropertiesDescriptor
    {
        public static QflowCustomPropertiesDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length < 3)
                throw new FormatException("Descriptor is invalid.");

            if (parts[0] != "qflow")
                throw new FormatException("String was not a qflow custom properties descriptor.");

            return parts.Length > 3
                ? new QflowCustomPropertiesDescriptor(parts[1], parts[2], parts[3])
                : new QflowCustomPropertiesDescriptor(parts[1], parts[2], null);
        }

        public QflowCustomPropertiesDescriptor(string callCentreBooking, string appBooking, string? callCentreEmailAddress)
        {
            CallCentreBooking = callCentreBooking;
            AppBooking = appBooking;
            CallCentreEmailAddress = callCentreEmailAddress;
        }

        public string CallCentreBooking { get; set; } 
        public string AppBooking { get; set; }
        public string? CallCentreEmailAddress { get; set; }
    }
}
