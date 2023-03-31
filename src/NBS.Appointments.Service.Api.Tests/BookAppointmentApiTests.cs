using FluentAssertions;
using NBS.Appointments.Service.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class BookAppointmentApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/appointment/book";

        [Fact]
        public async Task BookAppointment_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task BookAppointment_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new BookAppointmentRequest
            {
                Slot = string.Empty,
                CustomerDetails = new CustomerDetails(),
                Properties = string.Empty
            };

            var payload = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BookAppointment_ShouldReturnOkResponse_WhenRequestModelIsValid()
        {

        }
    }
}
