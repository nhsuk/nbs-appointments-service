using FluentAssertions;
using NBS.Appointments.Service.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class RescheduleAppointmentApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/appointment/reschedule";

        [Fact]
        public async Task RescheduleAppointment_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task RescheduleAppointment_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new RescheduleAppointmentRequest
            {
                Appointment = "qflow:invalid:appointment:descriptor"
            };

            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RescheduleAppointment_ShouldReturnOkResponse_WhenRequestModelIsValid()
        {
            var request = new RescheduleAppointmentRequest
            {
                Appointment = "qflow:12:34:56:2023-04-29:545:3"
            };

            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
