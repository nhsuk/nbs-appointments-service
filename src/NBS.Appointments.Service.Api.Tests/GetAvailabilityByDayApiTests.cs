using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAvailabilityByDayApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/availability/days";

        [Fact]
        public async Task GetAvailability_RespondsWithUnsupportedMediaType_WhenJsonNotSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task GetAvailability_RespondsWithBadRequest_WhenMalformedPayloadIsSent()
        {
            var payload = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAvailability_RespondsOk_WithValidRequest()
        {
            var from = DateTime.Today;
            var until = from.AddDays(10);
            var payload = new ApiRequest(new[] { "qflow:1234" }, from.ToString("yyyy-MM-dd"), until.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        public async Task GetAvailability_RespondsWithBadRequest_WithDatesInPast(int fromDays, int untilDays)
        {
            var from = DateTime.Today.AddDays(fromDays);
            var until = from.AddDays(untilDays);
            var payload = new ApiRequest(new[] { "qflow:1234" }, from.ToString("yyyy-MM-dd"), until.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public async Task GetAvailability_RespondsWithBadRequest_WithInvalidRequestData(object payload)
        {
            var jsonContent= new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        public static IEnumerable<object[]> BadRequests()
        {
            var from = DateTime.Today.AddDays(1);
            var until = from.AddDays(10);
            yield return new object[] { new ApiRequest(new[] {"1234"}, from.ToString("yyyy-MM-dd"), until.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "otherflow:1234" }, from.ToString("yyyy-MM-dd"), until.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "qflow:1234" }, "not-a-date", until.ToString("yyyy-MM-dd"), "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "qflow:1234" }, from.ToString("yyyy-MM-dd"), "not-a-date", "qflow:0:12345:NotSet") };
            yield return new object[] { new ApiRequest(new[] { "qflow:1234" }, from.ToString("yyyy-MM-dd"), until.ToString("yyyy-MM-dd"), "bad-data") };
        }

        public record ApiRequest(string[] Sites, string From, string Until, string Service);
    }
}
