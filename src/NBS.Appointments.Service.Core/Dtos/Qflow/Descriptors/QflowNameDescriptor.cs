namespace NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors
{
    public class QflowNameDescriptor
    {
        public static QflowNameDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(':');

            if (parts.Length != 2)
                throw new FormatException("Descriptor is invalid.");

            return new QflowNameDescriptor(parts[0], parts[1]);
        }

        public QflowNameDescriptor(string firstName, string surname)
        {
            FirstName = firstName;
            Surname = surname;
        }

        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}
