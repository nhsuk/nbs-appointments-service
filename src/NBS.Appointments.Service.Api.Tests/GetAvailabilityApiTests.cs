using FluentAssertions;
using Newtonsoft.Json;
using System.Text;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAvailabilityApiTests
    {        
        [Fact]
        public async Task GetAvailability_RespondsWithUnsupportedMediaType_WhenJsonNotSpecified()
        {
            var httpClient = new HttpClient();
            var payload = new StringContent("");
            var response = await httpClient.PostAsync(GetEndpoint(), payload);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task GetAvailability_RespondsWithBadRequest_WhenMalformedPayloadIsSent()
        {
            var httpClient = new HttpClient();
            var payload = new StringContent("", Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(GetEndpoint(), payload);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAvailability_RespondsOk_WithValidRequest()
        {
            var httpClient = new HttpClient();
            var payload = new ApiRequest(new[] { "qflow:1234" }, "2023-05-20", "2023-05-25", "qflow:0:12345:NotSet");
            var jsonContent = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(GetEndpoint(), jsonContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Theory]
        [MemberData(nameof(BadRequests))]
        public async Task GetAvailability_RespondsWithBadRequest_WithInvalidRequestData(object payload)
        {
            var httpClient = new HttpClient();
            var jsonContent= new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(GetEndpoint(), jsonContent);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        private string GetEndpoint() => "http://localhost:4000/availability/query";

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
