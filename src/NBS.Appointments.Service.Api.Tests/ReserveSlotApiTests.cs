using FluentAssertions;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class ReserveSlotApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/slot/reserve";

        [Fact]
        public async Task ReserveSlot_RespondsWithUnsupportedMediaType_WhenJsonNotSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task ReserveSlot_RespondsWithBadRequest_WhenMalformedPayloadIsSent()
        {
            var payload = new StringContent("", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public async Task ReserveSlot_ReturnsBadRequest_WhenInvalidPayloadSent(object payload)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, jsonContent);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ReserveSlot_RespondsOk_WhenPayloadIsValid()
        {
            var payload = new ReserveSlotApiRequest("qflow:1:2:3:2023-04-05:4:5", 5);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Endpoint, jsonContent);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        public static IEnumerable<object[]> BadRequests()
        {
            yield return new object[] { new ReserveSlotApiRequest("someslot:1:2:3", 5) };
            yield return new object[] { new ReserveSlotApiRequest("qflow:n:2:3", 5) };
            yield return new object[] { new ReserveSlotApiRequest("qflow:1:n:3", 5) };
            yield return new object[] { new ReserveSlotApiRequest("qflow:1:2:n", 5) };
        }

        public record ReserveSlotApiRequest(string slot, int lockDuration);
    }
}
