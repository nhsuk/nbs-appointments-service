using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using NBS.Appointments.Service.Models;

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
