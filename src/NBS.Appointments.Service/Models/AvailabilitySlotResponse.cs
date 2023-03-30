using System.Collections.Generic;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using NBS.Appointments.Service.Core.Dtos.Qflow;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilitySlotResponse
    {
        [JsonPropertyName("siteId")]
        public string SiteId { get; set; }

        [JsonPropertyName("service")]
        public string Service { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("slots")]
        public List<SlotInfo> Slots { get; set; }

        public class SlotInfo
        {
            [JsonPropertyName("from")]
            public DateTime From { get; set; }

            [JsonPropertyName("duration")]
            public int Duration { get; set; }

            [JsonPropertyName("ref")]
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
                Reference = $"qflow:{qs.ServiceId}:{qs.CalendarId}:{qs.AppointmentTypeId}:{qs.Time.TotalMinutes}:{qs.Time.TotalMinutes + qs.Duration}"
            }).ToList()
        };        
    }
}
