using FluentAssertions;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAvailabilityByHourApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/availability/hours";

        [Fact]
        public async Task AvailabilityByHours_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task AvailabilityByHours_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new SiteAvailabilityRequest
            {
                Service = string.Empty,
                Date = DateTime.Today,
                Site = "siteId:150"
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AvailabilityByHours_ShouldReturnOkResponse_WhenRequestIsValid()
        {
            var request = new SiteAvailabilityRequest
            {
                Service = "qflow:0:FLU18TO65:NotSet",
                Date = DateTime.Today.AddDays(1),
                Site = "qflow:150"
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(0, 4)]
        [InlineData(5, 3)]
        [InlineData(20, 2)]
        [InlineData(35, 1)]
        [InlineData(50, 0)]
        public async Task GetAvailabilityByHours_RestictsSlotCountsToFutureSlots(int minutesPastTheHour, int expectedCount)
        {
            var mockApiClient = new MockApi.Client("http://localhost:4010");
            await mockApiClient
                .SetupOnce("GET", "/svcCustomAppointment.svc/rest/GetSiteDoseAvailability")
                .ReturnsJson(new SiteSlotsResponse
                {
                    SiteId = 1234,
                    VaccineType = "12345",
                    Availability = new List<SiteSlotAvailabilityResponse>
                    {
                        new SiteSlotAvailabilityResponse { AppointmentTypeId = 1, CalendarId = 1, ServiceId = 1, Duration = 5, Time = new TimeSpan(9,0,0)},
                        new SiteSlotAvailabilityResponse { AppointmentTypeId = 1, CalendarId = 1, ServiceId = 1, Duration = 5, Time = new TimeSpan(9,10,0)},
                        new SiteSlotAvailabilityResponse { AppointmentTypeId = 1, CalendarId = 1, ServiceId = 1, Duration = 5, Time = new TimeSpan(9,30,0)},
                        new SiteSlotAvailabilityResponse { AppointmentTypeId = 1, CalendarId = 1, ServiceId = 1, Duration = 5, Time = new TimeSpan(9,45,0)}
                    }
                });

            var serverTime = DateTime.Today.Add(new TimeSpan(9, minutesPastTheHour, 0));
            await mockApiClient
                .SetupOnce("GET", "/time")
                .Returns(200, serverTime.ToString("yyyy-MM-dd hh:mm"));

            var request = new SiteAvailabilityRequest
            {
                Service = "qflow:0:FLU18TO65:NotSet",
                Date = DateTime.Today.AddDays(1),
                Site = "qflow:150"
            };
            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            var actualResponseData = JsonConvert.DeserializeObject<AvailabilityHourResponse>(responseBody);

            actualResponseData.SiteId.Should().Be("qflow:150");
            if (expectedCount > 0)
            {
                actualResponseData.AvailabilityByHour.Count.Should().Be(1);
                actualResponseData.AvailabilityByHour.First().Hour.Should().Be("9");
                actualResponseData.AvailabilityByHour.First().Count.Should().Be(expectedCount);
            }
            else
                actualResponseData.AvailabilityByHour.Count.Should().Be(0);
        }
    }
}
