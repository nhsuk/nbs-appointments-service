using Newtonsoft.Json;

namespace NBS.Appointments.Service.Infrastructure.Dtos.Qflow
{
    public class AvailabilityResponse
    {
        public DateTime Date { get; set; }

        [JsonProperty("AM")]
        public long Am { get; set; }

        [JsonProperty("PM")]
        public long Pm { get; set; }
    }
}
