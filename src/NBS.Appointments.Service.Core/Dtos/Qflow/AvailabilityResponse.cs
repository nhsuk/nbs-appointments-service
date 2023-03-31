using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Dtos.Qflow
{
    public class AvailabilityResponse
    {
        public DateTime Date { get; set; }

        [JsonPropertyName("AM")]
        public int Am { get; set; }

        [JsonPropertyName("PM")]
        public int Pm { get; set; }
    }
}
