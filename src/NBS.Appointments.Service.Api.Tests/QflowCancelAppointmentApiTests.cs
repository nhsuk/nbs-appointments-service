﻿using FluentAssertions;
using NBS.Appointments.Service.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class QflowCancelAppointmentApiTests : ApiTestBase
    {        
        public override string PathToTest => "appointment/cancel";

        [Fact]
        public async Task CancelAppointment_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task CancelAppointment_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new CancelAppointmentRequest
            {
                Appointment = "invalid:appointment:descriptor",
                Cancelation = "invalid:cancelation:reason:descriptor"
            };
            
            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CancelAppointment_ShouldReturnOkResponse_WhenRequestModelIsValid()
        {
            var request = new CancelAppointmentRequest
            {
                Appointment = "qflow:123:123456:456",
                Cancelation = "qflow:654:321"
            };

            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CancelAppointment_ShouldReturnNotFound_WhenAppointmentProcessIdNotFound()
        {
            var request = new CancelAppointmentRequest
            {
                Appointment = "qflow:123:456:789",
                Cancelation = "qflow:654:321"
            };

            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await HttpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
