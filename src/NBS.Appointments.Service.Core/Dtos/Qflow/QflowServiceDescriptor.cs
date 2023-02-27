namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class QflowCovidServiceDescriptor
    {
        public static QflowCovidServiceDescriptor FromString(string descriptor)
        {
            var parts = descriptor.Split(":");
            if(parts[0] != "covid")
                throw new FormatException("String was not a covid service descriptor");

            return new QflowCovidServiceDescriptor(parts[1], parts[2], parts[3]);
        }        

        public QflowCovidServiceDescriptor(string dose, string vaccine, string reference)
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