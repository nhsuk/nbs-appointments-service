using Newtonsoft.Json;

namespace NBS.Appointments.Service.Infrastructure.Dtos.Qflow
{
    public class AvailabilityResponse
    {
        public DateTime Date { get; set; }

        [JsonProperty("AM")]
        public int Am { get; set; }

        [JsonProperty("PM")]
        public int Pm { get; set; }
    }
}
