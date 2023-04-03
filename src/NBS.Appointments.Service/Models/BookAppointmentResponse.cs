using NBS.Appointments.Service.Core.Dtos.Qflow;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class BookedAppointmentResponse
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        public static BookedAppointmentResponse FromQflowResponse(BookAppointmentResponse qflowResponse) => new BookedAppointmentResponse
        {
            Ref = $"qflow:{qflowResponse.AppointmentId}"
        };
    }
}
