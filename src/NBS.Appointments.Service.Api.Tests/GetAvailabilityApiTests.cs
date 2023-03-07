using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;
using NBS.Appointments.Service.Models;

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAvailabilityApiTests
    {
        private readonly HttpClient _httpClient = new();

        private const string BaseUrl = "http://localhost:4000/availability";
        private const string QueryUrl = $"{BaseUrl}/query";
        private const string HoursUrl = $"{BaseUrl}/hours";

        [Fact]
        public async Task GetAvailability_RespondsWithUnsupportedMediaType_WhenJsonNotSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(QueryUrl, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task GetAvailability_RespondsWithBadRequest_WhenMalformedPayloadIsSent()
        {
            var payload = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(QueryUrl, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAvailability_RespondsOk_WithValidRequest()
        {
            var payload = new ApiRequest(new[] { "qflow:1234" }, "2023-05-20", "2023-05-25", "qflow:0:12345:NotSet");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(QueryUrl, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public async Task GetAvailability_RespondsWithBadRequest_WithInvalidRequestData(object payload)
        {
            var jsonContent= new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(QueryUrl, jsonContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AvailabilityByHours_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(HoursUrl, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task AvailableSlotsAsync_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new AvailabilityByHourRequest
            {
                AppointmentType = string.Empty,
                Date = DateTime.Today,
                Site = "siteId:150"
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(HoursUrl, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task AvailableSlotsAsync_ShouldReturnOkResponse_WhenRequestIsValid()
        {
            var request = new AvailabilityByHourRequest
            {
                AppointmentType = "FLU18TO65",
                Date = DateTime.Today.AddDays(1),
                Site = "siteId:150"
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(HoursUrl, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new ApiRequest(new[] {"1234"}, "2023-05-20", "2023-05-25", "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "otherflow:1234" }, "2023-05-20", "2023-05-25", "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "qflow:1234" }, "not-a-date", "2023-05-25", "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "qflow:1234" }, "2023-05-25", "not-a-date", "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "qflow:1234" }, "2023-05-25", "2023-05-25", "bad-data") };
        }

        public record ApiRequest(string[] Sites, string From, string Until, string Service);
    }
}
