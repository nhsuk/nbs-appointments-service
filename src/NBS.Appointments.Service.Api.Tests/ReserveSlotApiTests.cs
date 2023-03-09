using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NBS.Appointments.Service.Api.Tests
{
    public class ReserveSlotApiTests
    {
        private readonly HttpClient _httpClient = new();
        private const string Endpoint = "localhost:4000/slot/reservation";

        [Fact]
        public async Task ReserveSlot_RespondsWithUnsupportedMediaType_WhenJsonNotSpecified()
        {

        }

        [Theory]
        [MemberData]
        public async Task ReserveSlot_ReturnsBadRequest_WhenInvalidPayloadSent()
        {

        }
    }
}
