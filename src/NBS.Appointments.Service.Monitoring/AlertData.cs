using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Monitoring;

public class AlertData
{
    [JsonPropertyName("data")] 
    public Data Data { get; set; }
}