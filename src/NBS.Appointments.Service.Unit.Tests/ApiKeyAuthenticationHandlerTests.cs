using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Text.Encodings.Web;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class ApiKeyAuthenticationHandlerTests
    {
        private const string ApiKeyHeader = "nbs-api-key";

        [Fact]
        public async Task HandleAuthenticateAsync_ReturnsFail_WhenApiKeyHeaderMissing()
        {
            var classUnderTest = ConfigureClassUnderTest(String.Empty);
            var result = await classUnderTest.TestAccessHandleAuthenticateAsync();
            result.Succeeded.Should().Be(false);
            result.Principal.Should().BeNull();
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ReturnsFail_WhenApiKeyHeaderContainsWrongValue()
        {
            var classUnderTest = ConfigureClassUnderTest("bad-value");
            var result = await classUnderTest.TestAccessHandleAuthenticateAsync();
            result.Succeeded.Should().Be(false);
            result.Principal.Should().BeNull();
        }

        [Fact]
        public async Task HandleAuthenticateAsync_ReturnsSuccess_WhenApiKeyHeaderContainsCorrectValue()
        {
            var classUnderTest = ConfigureClassUnderTest("test-key");
            var result = await classUnderTest.TestAccessHandleAuthenticateAsync();
            result.Succeeded.Should().Be(true);
            result.Principal.Should().NotBeNull();
        }

        private TestableApiKeyAuthenticationHandler ConfigureClassUnderTest(string clientHeaderValue)
        {
            var httpContext = new DefaultHttpContext();
            if(String.IsNullOrEmpty(clientHeaderValue) == false)
            {
                httpContext.Request.Headers.Add(ApiKeyHeader, clientHeaderValue);
            }            
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var mockSystemClock = new Mock<ISystemClock>();
            var mockOptionsMonitor = new Mock<IOptionsMonitor<ApiKeyAuthenticationOptions>>();
            mockOptionsMonitor.Setup(x => x.CurrentValue).Returns(new ApiKeyAuthenticationOptions { ApiKey = "test-key" });

            return new TestableApiKeyAuthenticationHandler(mockHttpContextAccessor.Object, mockLoggerFactory.Object, UrlEncoder.Default, mockSystemClock.Object, mockOptionsMonitor.Object);
        }
    }

    public class TestableApiKeyAuthenticationHandler : ApiKeyAuthenticationHandler
    {
        public TestableApiKeyAuthenticationHandler(
        IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock systemClock,
        IOptionsMonitor<ApiKeyAuthenticationOptions> options) : base(httpContextAccessor, loggerFactory, encoder, systemClock, options)
        {
            
        }

        public Task<AuthenticateResult> TestAccessHandleAuthenticateAsync()
        {
            return HandleAuthenticateAsync();
        }

        protected override string SchemeName => "TestScheme";
    }
        

}
