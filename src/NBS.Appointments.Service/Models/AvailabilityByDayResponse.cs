using System.Linq;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityByDayResponse
    {
        [JsonPropertyName("site")]
        public string Site { get; set; }
        [JsonPropertyName("service")]
        public string Service { get; set; }
        [JsonPropertyName("availability")] 
        public AvailabilityEntry[] Availability { get; set; }

        public class AvailabilityEntry
        {
            [JsonPropertyName("date")]
            public string Date { get; set; }
            [JsonPropertyName("AM")]
            public int Am { get; set; }
            [JsonPropertyName("PM")]
            public int Pm { get; set; }
        }

        public static AvailabilityByDayResponse FromQflowResponse(Dtos.Qflow.SiteAvailabilityResponse qflowResponse, string service) => new AvailabilityByDayResponse
        {
            Site = $"qflow:{qflowResponse.SiteId}",
            Service = service,
            Availability = qflowResponse.Availability.Select(av => new AvailabilityEntry
            {
                Date = av.Date.ToString("yyyy-MM-dd"),
                Am = av.Am,
                Pm = av.Pm
            }).ToArray()
        };
        
    }
}
