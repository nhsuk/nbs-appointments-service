using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Monitoring;

public class Condition
{
    [JsonPropertyName("allOf")] 
    public ConditionDetails[] ConditionDetails { get; set; }
}