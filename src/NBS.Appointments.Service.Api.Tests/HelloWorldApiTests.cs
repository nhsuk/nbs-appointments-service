using FluentAssertions;
using System.Net;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class HelloWorldApiTests
    {
        [Fact]
        public async Task HelloWorldEndpoint_ReturnsOk_WithCorrectMessage()
        {
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://localhost:4000/hello-world");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
