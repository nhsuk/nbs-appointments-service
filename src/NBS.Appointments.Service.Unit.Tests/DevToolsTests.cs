using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace NBS.Appointments.Service.Unit.Tests
{
    public class DevToolsTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public DevToolsTests(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task RaiseException_ShouldThrowExceptionWithDetailsInResponseWhenShowExceptionTrue()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, configbuilder) =>
                {
                    configbuilder.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["ShowException"] = "true",
                            ["ApiKey"] = "secret"
                        });
                });
            }).CreateClient();

            client.DefaultRequestHeaders.Add("nbs-api-key", "secret");

            var response = await client.GetAsync("devtools/raise-exception");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Exception of type 'System.Exception' was thrown.");
        }

        [Fact]
        public async Task RaiseException_ShouldThrowExceptionWithOutAnyDetailsInResponseWhenShowExceptionFalse()
        {
            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, configbuilder) =>
                {
                    configbuilder.AddInMemoryCollection(
                        new Dictionary<string, string>
                        {
                            ["ShowException"] = "false",
                            ["ApiKey"] = "secret"
                        });
                });
            }).CreateClient();

            client.DefaultRequestHeaders.Add("nbs-api-key", "secret");

            var response = await client.GetAsync("devtools/raise-exception");
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Be("");
        }
    }
}
