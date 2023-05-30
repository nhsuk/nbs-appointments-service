using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Monitoring;

public class Essentials
{
    [JsonPropertyName("alertRule")] 
    public string AlertRule { get; set; }
    [JsonPropertyName("monitorCondition")] 
    public string MonitorCondition { get; set; }
    [JsonPropertyName("alertTargetIDs")] 
    public string[] AlertTargetIDs { get; set; }
}