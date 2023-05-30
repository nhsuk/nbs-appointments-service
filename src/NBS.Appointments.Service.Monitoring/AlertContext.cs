using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Monitoring;

public class AlertContext
{
    [JsonPropertyName("condition")] 
    public Condition Condition { get; set; }
}