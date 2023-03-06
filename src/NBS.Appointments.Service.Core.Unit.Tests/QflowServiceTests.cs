using Microsoft.Extensions.Options;
using Moq;
using NBS.Appointments.Service.Core.Services;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests
{
    public class QflowServiceTests
    {
        [Fact]
        private void GetSiteAvailability_ThrowsArgumentException_WhenNoSitesAreProvided()
        {
            var mockOptions = new Mock<IOptions<QflowOptions>>();
            mockOptions.Setup(x => x.Value).Returns(new QflowOptions { BaseUrl = "http://test", UserName = "testName", Password = "testPassword" });

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockSessionManager = new Mock<IQflowSessionManager>();

            var sut = new QflowService(mockOptions.Object, mockHttpClientFactory.Object, mockSessionManager.Object);
            Assert.ThrowsAsync<ArgumentException>(() =>
                sut.GetSiteAvailability(Enumerable.Empty<string>(), DateTime.Today, DateTime.Today.AddDays(2), "0", "12345", ""));
        }

        [Fact]
        private void GetSiteAvailability_ThrowsArgumentException_WhenInvalidDateRangeSupplied()
        {
            var mockOptions = new Mock<IOptions<QflowOptions>>();
            mockOptions.Setup(x => x.Value).Returns(new QflowOptions { BaseUrl = "http://test", UserName = "testName", Password = "testPassword" });

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockSessionManager = new Mock<IQflowSessionManager>();

            var sut = new QflowService(mockOptions.Object, mockHttpClientFactory.Object, mockSessionManager.Object);
            Assert.ThrowsAsync<ArgumentException>(() =>
                sut.GetSiteAvailability(new[] {"site1"}, DateTime.Today, DateTime.Today.AddDays(-2), "0", "12345", ""));
        }

        [Fact]
        private void GetSiteAvailability_ThrowsArgumentException_WhenNoDoseIsProvided()
        {
            var mockOptions = new Mock<IOptions<QflowOptions>>();
            mockOptions.Setup(x => x.Value).Returns(new QflowOptions { BaseUrl = "http://test", UserName = "testName", Password = "testPassword" });

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockSessionManager = new Mock<IQflowSessionManager>();

            var sut = new QflowService(mockOptions.Object, mockHttpClientFactory.Object, mockSessionManager.Object);
            Assert.ThrowsAsync<ArgumentException>(() =>
                sut.GetSiteAvailability(new[] { "site1" }, DateTime.Today, DateTime.Today.AddDays(2), "", "12345", ""));
        }

        [Fact]
        private void GetSiteAvailability_ThrowsArgumentException_WhenNoVaccineIsProvided()
        {
            var mockOptions = new Mock<IOptions<QflowOptions>>();
            mockOptions.Setup(x => x.Value).Returns(new QflowOptions { BaseUrl = "http://test", UserName = "testName", Password = "testPassword" });

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            var mockSessionManager = new Mock<IQflowSessionManager>();

            var sut = new QflowService(mockOptions.Object, mockHttpClientFactory.Object, mockSessionManager.Object);
            Assert.ThrowsAsync<ArgumentException>(() =>
                sut.GetSiteAvailability(new[] { "site1" }, DateTime.Today, DateTime.Today.AddDays(2), "0", "", ""));
        }        
    }
}