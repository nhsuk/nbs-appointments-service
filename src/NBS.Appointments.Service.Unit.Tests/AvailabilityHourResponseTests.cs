using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Models;
using Xunit;
using static NBS.Appointments.Service.Models.AvailabilityHourResponse;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class AvailabilityHourResponseTests
    {
        [Fact]
        public void FromQflowResponse_MapsDataCorrectly()
        {
            var testData = new SiteSlotsResponse
            {
                SiteId = 123,
                VaccineType = "39115611000001103",
                Availability = new List<Core.Dtos.Qflow.SiteSlotAvailabilityResponse>
                {
                    new Core.Dtos.Qflow.SiteSlotAvailabilityResponse
                    {
                        AppointmentTypeId = 1,
                        CalendarId = 1,
                        Duration = 5,
                        ServiceId = 1,
                        Time = new TimeSpan(9, 0, 0)
                    },
                    new Core.Dtos.Qflow.SiteSlotAvailabilityResponse
                    {
                        AppointmentTypeId = 1,
                        CalendarId = 1,
                        Duration = 5,
                        ServiceId = 1,
                        Time = new TimeSpan(10, 0, 0)
                    },
                    new Core.Dtos.Qflow.SiteSlotAvailabilityResponse
                    {
                        AppointmentTypeId = 1,
                        CalendarId = 1,
                        Duration = 5,
                        ServiceId = 1,
                        Time = new TimeSpan(11, 0, 0)
                    }
                }
            };

            var expected = new Models.AvailabilityHourResponse
            {
                SiteId = "123",
                Date = DateTime.Today,
                Type = "39115611000001103",
                AvailabilityByHour = new List<AvailabilityHour>
                {
                    new AvailabilityHour("9", 1),
                    new AvailabilityHour("10", 1),
                    new AvailabilityHour("11", 1)
                }
            };

            var actual = Models.AvailabilityHourResponse.FromQflowResponse(testData, testData.VaccineType, DateTime.Today);

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
