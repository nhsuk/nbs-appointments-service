using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CreateCustomerPayload : BasePayload
    {
        [JsonProperty("customer")]
        public Customer Customer { get; set; }
    }

    public class Customer
    {
        [JsonProperty("active")]
        public bool Active => true;

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("PersonalId")]
        public string NHSNumber { get; set; }

        [JsonProperty("SelfReferalOccupation")]
        public string SelfReferralOccupation { get; set; }

        [JsonProperty("DoB")]
        public string DoB { get; set; }

        [JsonProperty("EMail")]
        public string? Email { get; set; }

        [JsonProperty("TelNumber1")]
        public string? TelNumber1 { get; set; }

        [JsonProperty("TelNumber2")]
        public string? TelNumber2 { get; set; }

        [JsonProperty("NotificationConsent")]
        public bool NotificationConsent { get; set; }
    }
}
