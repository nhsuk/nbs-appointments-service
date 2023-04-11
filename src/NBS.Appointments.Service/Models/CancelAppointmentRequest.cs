using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class CancelAppointmentRequest
    {
        [JsonPropertyName("appointment")]
        public string Appointment { get; set; }
        [JsonPropertyName("cancellation")]
        public string Cancelation { get; set; }
    }
}
