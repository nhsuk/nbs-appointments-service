﻿using FluentAssertions;
using System.Net;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class GetAllCustomerAppointmentsApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "http://localhost:4000/appointment/get-all";

        [Fact]
        public async Task GetAllAppointments_ShouldReturnBadRequest_WhenNhsNumberNotProvided()
        {
            var requestUrl = $"{Endpoint}?nhsNumber=&includePastAppointments=true";
            var response = await _httpClient.GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetAllAppointments_ShouldReturnOk_WhenNhsNumberPresent()
        {
            var requestUrl = $"{Endpoint}?nhsNumber=123456789&includePastAppointments=true";
            var response = await _httpClient.GetAsync(requestUrl);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
