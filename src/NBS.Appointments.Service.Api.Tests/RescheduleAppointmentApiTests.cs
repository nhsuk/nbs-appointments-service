using FluentAssertions;
using NBS.Appointments.Service.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class RescheduleAppointmentApiTests : ApiTestBase
    {
        public override string PathToTest => "appointment/reschedule";

        [Fact]
        public async Task RescheduleAppointment_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task RescheduleAppointment_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new RescheduleAppointmentRequest
            {
                OriginalAppointment = "qflow:invalid:appointment:descriptor"
            };

            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task RescheduleAppointment_ShouldReturnOkResponse_WhenRequestModelIsValid()
        {
            var request = new RescheduleAppointmentRequest
            {
                OriginalAppointment = "qflow:12:34:56",
                RescheduledSlot = "qflow:1080:109429:1:2023-03-31:605:610"
            };

            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
