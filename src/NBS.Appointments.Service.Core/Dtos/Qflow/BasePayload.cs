using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class BasePayload
    {
        [JsonPropertyName("apiSessionId")]
        public string? ApiSessionId { get; set; }
    }
}
