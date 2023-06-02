using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Monitoring;

public class ConditionDetails
{
    [JsonPropertyName("metricName")] 
    public string MetricName { get; set; }
    [JsonPropertyName("operator")] 
    public string Operator { get; set; }
    [JsonPropertyName("threshold")] 
    public string Threshold { get; set; }
}