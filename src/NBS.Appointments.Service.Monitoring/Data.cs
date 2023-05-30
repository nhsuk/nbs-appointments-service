using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Monitoring;

public class Data
{
    [JsonPropertyName("essentials")] 
    public Essentials Essentials { get; set; }
    [JsonPropertyName("alertContext")] 
    public AlertContext AlertContext { get; set; }
}