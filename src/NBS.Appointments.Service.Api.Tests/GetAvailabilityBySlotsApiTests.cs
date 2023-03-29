using Newtonsoft.Json;
using System.Net;
using System.Text;
using FluentAssertions;
using Xunit;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Dtos.Qflow;

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAvailabilityBySlotsApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/availability/slots";

        [Fact]
        public async Task GetAvailabilityBySlots_RespondsWithUnsupportedMediaType_WhenJsonNotSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task GetAvailabilityBySlots_RespondsWithBadRequest_WhenMalformedPayloadIsSent()
        {
            var payload = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAvailabilityBySlots_RespondsOk_WithValidRequest()
        {
            var from = DateTime.Today;            
            var payload = new ApiRequest("qflow:1234",  from.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            var actualResponseData = JsonConvert.DeserializeObject<AvailabilitySlotResponse>(responseBody);

            actualResponseData.SiteId.Should().Be("qflow:1234");
            actualResponseData.Service.Should().Be("qflow:0:12345:NotSet");
        }

        [Theory]
        [InlineData(855, 4)]
        [InlineData(905, 3)]
        [InlineData(915, 2)]
        [InlineData(1005, 1)]
        [InlineData(1100, 0)]
        public async Task GetAvailabilityBySlots_RestictsSlotsToFutureSlots(int time, int expectedCount)
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
                        new SiteSlotAvailabilityResponse { AppointmentTypeId = 1, CalendarId = 1, ServiceId = 1, Duration = 5, Time = new TimeSpan(10,0,0)},
                        new SiteSlotAvailabilityResponse { AppointmentTypeId = 1, CalendarId = 1, ServiceId = 1, Duration = 5, Time = new TimeSpan(10,10,0)}
                    }
                });

            var serverTime = DateTime.Today.Add(new TimeSpan(time/100, time%100, 0));
            await mockApiClient
                .SetupOnce("GET", "/time")
                .Returns(200, serverTime.ToString("yyyy-MM-dd hh:mm"));

            var from = DateTime.Today;
            var payload = new ApiRequest("qflow:1234", from.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseBody = await response.Content.ReadAsStringAsync();
            var actualResponseData = JsonConvert.DeserializeObject<AvailabilitySlotResponse>(responseBody);

            actualResponseData.SiteId.Should().Be("qflow:1234");
            actualResponseData.Service.Should().Be("qflow:0:12345:NotSet");
            actualResponseData.Slots.Should().HaveCount(expectedCount);
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public async Task GetAvailability_RespondsWithBadRequest_WithInvalidRequestData(object payload)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static IEnumerable<object[]> BadRequests()
        {
            var from = DateTime.Today.AddDays(1);            
            yield return new object[] { new ApiRequest( "1234" , from.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest("otherflow:1234" , from.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest( "qflow:1234" , "not-a-date", "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest("qflow:1234" , from.ToString("yyyy-MM-dd"), "bad-data") };
        }

        public record ApiRequest(string Site, string Date, string Service);
    }
}
