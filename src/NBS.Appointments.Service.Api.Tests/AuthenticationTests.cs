﻿using FluentAssertions;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class AuthenticationTests : ApiTestBase
    {
        public override string PathToTest => throw new NotImplementedException();

        [Theory]
        [MemberData(nameof(EndPoints))]
        public async Task Endpoints_Return401UnAuthorized_WhenNoApiKeyIsSupplied(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, $"{BaseUrl}/{endpoint}");
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        [Theory]
        [MemberData(nameof(EndPoints))]
        public async Task Endpoints_Return401UnAuthorized_WhenIncorrectApiKeyIsSupplied(HttpMethod method, string endpoint)
        {
            var request = new HttpRequestMessage(method, $"{BaseUrl}/{endpoint}");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("nbs-api-key", "incorrect");
            var response = await httpClient.SendAsync(request);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
        }

        public static IEnumerable<object[]> EndPoints()
        {
            yield return new object[] { HttpMethod.Post, "availability/days" };
            yield return new object[] { HttpMethod.Post, "availability/hours" };
            yield return new object[] { HttpMethod.Post, "availability/slots" };
            yield return new object[] { HttpMethod.Post, "slot/reserve" };
            yield return new object[] { HttpMethod.Post, "appointment/book" };
            yield return new object[] { HttpMethod.Post, "appointment/cancel" };
            yield return new object[] { HttpMethod.Get, "appointment/get-all" };
        }
    }
}
