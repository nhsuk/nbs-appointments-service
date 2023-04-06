using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class CancelAppointmentRequest
    {
        [JsonPropertyName("appointment")]
        public string Appointment { get; set; }
        [JsonPropertyName("cancellation")]
        public string Cancellation { get; set; }
    }
}
