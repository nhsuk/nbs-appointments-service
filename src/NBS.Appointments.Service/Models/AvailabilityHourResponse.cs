using NBS.Appointments.Service.Core.Dtos.Qflow;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NBS.Appointments.Service.Models
{
    public class AvailabilityHourResponse
    {
        [JsonProperty("siteId")]
        public string SiteId { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("availabilityByHour")]
        public List<AvailabilityHour> AvailabilityByHour { get; set; }

        public class AvailabilityHour
        {
            public AvailabilityHour(string hour, int count)
            {
                Hour = hour;
                Count = count;
            }

            [JsonProperty("hour")]
            public string Hour { get; set; }
            [JsonProperty("count")]
            public int Count { get; set; }
        }

        public static AvailabilityHourResponse FromQflowResponse(SiteSlotsResponse qflowResponse, string vaccineType, DateTime requestDate, DateTime currentDate)
        {
            var response = new AvailabilityHourResponse
            {
                Date = requestDate,
                SiteId = qflowResponse.SiteId.ToString(),
                Type = vaccineType
            };

            var availableSlotTimes = qflowResponse.Availability
                .GroupBy(x => x.Time)
                .Select(x => x.Key)
                .ToList();

            var availableHours = qflowResponse.Availability
                .GroupBy(x => x.Time.Hours)
                .Select(x => x.Key)
                .ToList();

            var availabilityByHour = new List<AvailabilityHour>();

            if (!availableSlotTimes.Any())
            {
                response.AvailabilityByHour = availabilityByHour;
                return response;
            }

            var currentTime = currentDate.TimeOfDay;

            var dateDifference = DateTime.Compare(requestDate.Date, currentDate.Date);

            foreach (var hour in availableHours)
            {
                if (dateDifference < 0)
                    continue;

                int slotTimesInHour;

                if (dateDifference == 0)
                {
                    slotTimesInHour = availableSlotTimes
                        .Where(x => x.Hours == hour && x > currentTime)
                        .Count();
                }
                else
                {
                    slotTimesInHour = availableSlotTimes
                        .Where(x => x.Hours == hour)
                        .Count();
                }

                if (slotTimesInHour == 0)
                    continue;

                availabilityByHour.Add(new AvailabilityHour(hour.ToString(), slotTimesInHour));
            }

            response.AvailabilityByHour = availabilityByHour;
            return response;
        }
    }
}
