using Newtonsoft.Json;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityResponse
    {
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("service")]
        public string Service { get; set; }
        [JsonProperty("availability")] 
        public Availability[] Availability { get; set; }
    }
}
