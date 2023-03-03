using FluentAssertions;
using NBS.Appointments.Service.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class SlotControllerApiTests
    {
        private HttpClient _httpClient = new HttpClient();
        private const string Endpoint = "http://localhost:4001/slots/available-slots";

        [Fact]
        public async Task AvailableSlotsAsync_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new SlotAvailabilityRequest
            {
                AppointmentType = string.Empty,
                Date = DateTime.Today,
                SiteIdentifier = "siteId:150"
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
