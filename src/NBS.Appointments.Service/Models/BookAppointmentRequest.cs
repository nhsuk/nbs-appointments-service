using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class BookAppointmentRequest
    {
        [JsonPropertyName("slot")]
        public string Slot { get; set; }
        [JsonPropertyName("customerDetails")]
        public CustomerDetails CustomerDetails { get; set; }
        [JsonPropertyName("properties")]
        public string Properties { get; set; }
    }

    public class CustomerDetails
    {
        [JsonPropertyName("nhsNumber")]
        public string NhsNumber { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("dob")]
        public string Dob { get; set; }
        [JsonPropertyName("contactDetails")]
        public ContactDetails ContactDetails { get; set; }
        [JsonPropertyName("qualifiers")]
        public string Qualifiers { get; set; }
        [JsonPropertyName("selfReferralOccupation")]
        public string? SelfReferralOccupation { get; set; }
    }

    public class ContactDetails
    {
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("landline")]
        public string Landline { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
