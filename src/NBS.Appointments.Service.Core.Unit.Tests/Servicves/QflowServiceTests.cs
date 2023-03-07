using Microsoft.Extensions.Options;
using Moq;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Core.Services;
using Xunit;

namespace NBS.Appointments.Service.Core.Unit.Tests.Servicves
{
    public class QflowServiceTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
        private readonly Mock<IQflowSessionManager> _sessionManagerMock = new();
        private readonly Mock<IOptions<QflowOptions>> _optionsMock = new();

        private readonly IQflowService _sut;

        public QflowServiceTests()
        {
            _optionsMock.Setup(x => x.Value)
                .Returns(new QflowOptions
                { 
                    BaseUrl = "http://test",
                    UserName = "testName",
                    Password = "testPassword"
                });

            _sut = new QflowService(
                _optionsMock.Object,
                _httpClientFactoryMock.Object,
                _sessionManagerMock.Object);
        }

        [Fact]
        public void GetSiteAvailability_ThrowsArgumentException_WhenNoSitesAreProvided()
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.GetSiteAvailability(Enumerable.Empty<string>(), DateTime.Today, DateTime.Today.AddDays(2), "0", "12345", ""));
        }

        [Fact]
        public void GetSiteAvailability_ThrowsArgumentException_WhenInvalidDateRangeSupplied()
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.GetSiteAvailability(new[] { "site1" }, DateTime.Today, DateTime.Today.AddDays(-2), "0", "12345", ""));
        }

        [Fact]
        public void GetSiteAvailability_ThrowsArgumentException_WhenNoDoseIsProvided()
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.GetSiteAvailability(new[] { "site1" }, DateTime.Today, DateTime.Today.AddDays(2), "", "12345", ""));
        }

        [Fact]
        public void GetSiteAvailability_ThrowsArgumentException_WhenNoVaccineIsProvided()
        {
            Assert.ThrowsAsync<ArgumentException>(() =>
                _sut.GetSiteAvailability(new[] { "site1" }, DateTime.Today, DateTime.Today.AddDays(2), "0", "", ""));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void GetSiteSlotAvailabilityAsync_ThrowsArgumentException_WhenNoAppointmentTypeProvided(string appointmentType)
        {
            Assert.ThrowsAsync<ArgumentException>(() => _sut.GetSiteSlotAvailabilityAsync(123, DateTime.Today.AddDays(2), appointmentType));
        }
    }
}