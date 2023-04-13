using FluentAssertions;
using NBS.Appointments.Service.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class BookAppointmentApiTests
    {
        private readonly IHttpClientFactory _httpClientFactory = new ApiHttpClientFactory();
        private const string Endpoint = "http://localhost:4000/appointment/book";        

        [Fact]
        public async Task BookAppointment_ShouldReturnUnsupportedMediaType_WhenNoJsonSpecified()
        {
            var payload = new StringContent("");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.UnsupportedMediaType);
        }

        [Fact]
        public async Task BookAppointment_ShouldReturnBadRequest_WhenRequestModelFailsValidation()
        {
            var request = new BookAppointmentRequest
            {
                Slot = "invalid:slot:descriptor",
                CustomerDetails = new CustomerDetails
                {
                    Name = "Test"
                },
                Properties = string.Empty
            };

            var httpClient = _httpClientFactory.CreateClient();
            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BookAppointment_ShouldReturnOkResponse_WhenRequestModelIsValid()
        {
            var request = new BookAppointmentRequest
            {
                Slot = "qflow:1080:109429:1:2023-03-31:605:49",
                CustomerDetails = new CustomerDetails
                {
                    NhsNumber = "987654321",
                    Dob = DateTime.Today.AddYears(-35).ToShortDateString(),
                    Name = "Bruce:Wayne",
                    Qualifiers = string.Empty,
                    SelfReferralOccupation = string.Empty,
                    ContactDetails = new ContactDetails
                    {
                        Email = string.Empty,
                        Landline = string.Empty,
                        PhoneNumber = string.Empty
                    }
                },
                Properties = "qflow:1:0"
            };

            var httpClient = _httpClientFactory.CreateClient();
            var payload = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(Endpoint, payload);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }

    public class ApiHttpClientFactory : IHttpClientFactory
    {
        public HttpClient CreateClient(string name)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("nbs-api-key", "supersecret");
            return client;
        }
    }
}
