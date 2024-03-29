﻿using FluentAssertions;
using Xunit;
using NBS.Appointments.Service.Models;
using NBS.Appointments.Service.Dtos.Qflow;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class AvailabilityQueryResponseTests
    {
        [Fact]
        public void FromQflowResponse_MapsDataCorrectly()
        {
            SiteAvailabilityResponse testData = new SiteAvailabilityResponse
            {
                SiteId = 12,
                SiteAddress = "Address",
                SiteName = "Name",
                VaccineType = "123156",
                Availability = new AvailabilityResponse[]
                {
                    new AvailabilityResponse
                    {
                        Date = new DateTime(2023, 01, 01),
                        Am = 10,
                        Pm = 20
                    }
                }
            };

            var actual = AvailabilityByDayResponse.FromQflowResponse(testData, "test-service");

            var expected = new AvailabilityByDayResponse
            {
                Site = "qflow:12",
                Service = "test-service",
                Availability = new AvailabilityByDayResponse.AvailabilityEntry[]
                {
                    new AvailabilityByDayResponse.AvailabilityEntry
                    {
                        Date = "2023-01-01",
                        Am = 10,
                        Pm = 20
                    }
                }
            };

            actual.Should().BeEquivalentTo(expected);
        }
    }
}
