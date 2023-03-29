using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using System.Linq;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilitySlotResponse
    {
        [JsonProperty("siteId")]
        public string SiteId { get; set; }

        [JsonProperty("service")]
        public string Service { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("slots")]
        public List<SlotInfo> Slots { get; set; }

        public class SlotInfo
        {
            [JsonProperty("from")]
            public DateTime From { get; set; }

            [JsonProperty("duration")]
            public int Duration { get; set; }

            [JsonProperty("ref")]
            public string Reference { get; set; }
        }        
        
        public static AvailabilitySlotResponse FromQflowResponse(string site, string service, DateTime date, IEnumerable<SiteSlotAvailabilityResponse> slots) => new AvailabilitySlotResponse
        {
            Service = service,
            SiteId = site,
            Date = date,
            Slots = slots.Select(qs => new SlotInfo
            {
                From = date.Date.Add(qs.Time),
                Duration = qs.Duration,
                Reference = $"qflow:{qs.CalendarId}:{qs.Time.TotalMinutes}:{qs.Time.TotalMinutes + qs.Duration}"
            }).ToList()
        };        
    }
}
