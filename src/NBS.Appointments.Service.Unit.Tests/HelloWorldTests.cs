using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NBS.Appointments.Service.Functions;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class HelloWorldTests
    {
        private readonly Mock<ILogger> _loggerMock = new Mock<ILogger>();
        private readonly Mock<HttpRequest> _requestMock = new Mock<HttpRequest>();

        [Fact]
        public async Task HelloWorld_Run_ShouldReturnExpectedMessage()
        {
            const string expectedResponseMsg = "Hello world!";

            var result = await HelloWorld.Run(_requestMock.Object, _loggerMock.Object) as OkObjectResult;

            result.Should().NotBeNull();
            result.Value.Should().Be(expectedResponseMsg);
        }
    }
}
