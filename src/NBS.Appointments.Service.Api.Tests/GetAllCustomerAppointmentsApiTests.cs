using FluentAssertions;
using System.Net;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAllCustomerAppointmentsApiTests : ApiTestBase
    {
        public override string PathToTest => "appointment/get-all";

        [Fact]
        public async Task GetAllAppointments_ShouldReturnBadRequest_WhenNhsNumberNotProvided()
        {            
            var requestUrl = $"{Endpoint}?nhsNumber=&includePastAppointments=true";
            var response = await HttpClient.GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAllAppointments_ShouldReturnOk_WhenNhsNumberPresent()
        {
            var requestUrl = $"{Endpoint}?nhsNumber=123456789&includePastAppointments=true";
            var response = await HttpClient.GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
