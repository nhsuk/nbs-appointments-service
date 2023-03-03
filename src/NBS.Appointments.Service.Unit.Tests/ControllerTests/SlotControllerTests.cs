using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NBS.Appointments.Service.Controllers;
using NBS.Appointments.Service.Core.Interfaces.Services;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Validators;
using System.Net;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests.ControllerTests
{
    public class SlotControllerTests
    {
        private Mock<IQflowService> mockQflowService = new();
        private SlotAvailabilityRequestValidator _validator = new();

        private SlotController _sut;

        public SlotControllerTests()
        {
            _sut = new SlotController(
                mockQflowService.Object,
                _validator);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task AvailableSlots_ShouldFailValidation_WhenSiteIdentifierNotProvided(string siteIdentifier)
        {
            var request = new SlotAvailabilityRequest
            {
                SiteIdentifier = siteIdentifier,
                AppointmentType = "COVID18TO65",
                Date = DateTime.Today.AddDays(1)
            };

            var result = await _sut.AvailableSlotsAsync(request) as BadRequestObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var errorMsgs = result.Value as List<string>;

            errorMsgs.Should().NotBeNull();
            errorMsgs.Count.Should().Be(1);
            errorMsgs.First().Should().Be("Site identifier must be provided.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task AvailableSlots_ShouldFailValidation_WhenAppointmentTypeNotProvided(string appointmentType)
        {
            var request = new SlotAvailabilityRequest
            {
                SiteIdentifier = "siteId:123",
                AppointmentType = appointmentType,
                Date = DateTime.Today.AddDays(1)
            };
            
            var result = await _sut.AvailableSlotsAsync(request) as BadRequestObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var errorMsgs = result.Value as List<string>;

            errorMsgs.Should().NotBeNull();
            errorMsgs.Count.Should().Be(1);
            errorMsgs.First().Should().Be("Appointment type must be provided.");
        }

        [Fact]
        public async Task AvailableSlots_ShouldFailValidation_DateIsNotValid()
        {
            var request = new SlotAvailabilityRequest
            {
                SiteIdentifier = "siteId:123",
                AppointmentType = "COVID18TO65",
                Date = default
            };

            var result = await _sut.AvailableSlotsAsync(request) as BadRequestObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var errorMsgs = result.Value as List<string>;

            errorMsgs.Should().NotBeNull();
            errorMsgs.Count.Should().Be(3);
            errorMsgs.First().Should().Be("A date must be provided.");
            errorMsgs[1].Should().Be("The date you provided must be a valid date.");
            errorMsgs[2].Should().Be("The date must be in the future.");
        }

        [Fact]
        public async Task AvailableSlots_ShouldFailValidation_DateIsInThePast()
        {
            var request = new SlotAvailabilityRequest
            {
                SiteIdentifier = "siteId:123",
                AppointmentType = "COVID18TO65",
                Date = DateTime.Today.AddDays(-1)
            };

            var result = await _sut.AvailableSlotsAsync(request) as BadRequestObjectResult;

            result.Should().NotBeNull();
            result.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);

            var errorMsgs = result.Value as List<string>;

            errorMsgs.Should().NotBeNull();
            errorMsgs.Count.Should().Be(1);
            errorMsgs.First().Should().Be("The date must be in the future.");
        }
    }
}
