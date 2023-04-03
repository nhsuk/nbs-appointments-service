using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CreateCustomerPayload : BasePayload
    {
        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        [JsonPropertyName("active")]
        public bool Active => true;

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("PersonalId")]
        public string NHSNumber { get; set; }

        [JsonPropertyName("SelfReferalOccupation")]
        public string SelfReferralOccupation { get; set; }

        [JsonPropertyName("DoB")]
        public string DoB { get; set; }

        [JsonPropertyName("EMail")]
        public string? Email { get; set; }

        [JsonPropertyName("TelNumber1")]
        public string? TelNumber1 { get; set; }

        [JsonPropertyName("TelNumber2")]
        public string? TelNumber2 { get; set; }

        [JsonPropertyName("NotificationConsent")]
        public bool NotificationConsent { get; set; }
    }
}
