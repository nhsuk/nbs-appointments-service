using System.Linq;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityByDaysResponse
    {
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("service")]
        public string Service { get; set; }
        [JsonProperty("availability")] 
        public AvailabilityEntry[] Availability { get; set; }

        public class AvailabilityEntry
        {
            [JsonProperty("date")]
            public string Date { get; set; }
            [JsonProperty("AM")]
            public int Am { get; set; }
            [JsonProperty("PM")]
            public int Pm { get; set; }
        }

        public static AvailabilityByDaysResponse FromQflowResponse(Dtos.Qflow.SiteAvailabilityResponse qflowResponse, string service) => new AvailabilityByDaysResponse
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
