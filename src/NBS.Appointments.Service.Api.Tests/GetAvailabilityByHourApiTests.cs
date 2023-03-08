using FluentAssertions;
using NBS.Appointments.Service.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

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
        public async Task AvailableSlotsAsync_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new AvailabilityByHourRequest
            {
                AppointmentType = string.Empty,
                Date = DateTime.Today,
                Site = "siteId:150"
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

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
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
