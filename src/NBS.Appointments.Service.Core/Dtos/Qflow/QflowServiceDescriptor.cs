namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class QflowServiceDescriptor
    {
        public static QflowServiceDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(":");

            if (parts.Length != 4)
                throw new FormatException("Descriptor is not formatted correctly");

            if(parts[0] != "qflow")
                throw new FormatException("String was not a qflow service descriptor");

            return new QflowServiceDescriptor(parts[1], parts[2], parts[3]);
        }        

        public QflowServiceDescriptor(string dose, string vaccine, string reference)
        {
            Dose = dose;
            Vaccine = vaccine;
            Reference = reference;
        }

        public string Dose { get; init; }
        public string Vaccine {get;init;}
        public string Reference {get;init;}
    }
}
