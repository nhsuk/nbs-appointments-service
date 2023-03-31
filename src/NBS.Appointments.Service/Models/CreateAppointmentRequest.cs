using Newtonsoft.Json;
using System;

namespace NBS.Appointments.Service.Models
{
    public class BookAppointmentRequest
    {
        [JsonProperty("slot")]
        public string Slot { get; set; }
        [JsonProperty("customerDetails")]
        public CustomerDetails CustomerDetails { get; set; }
        [JsonProperty("properties")]
        public string Properties { get; set; }
    }

    public class CustomerDetails
    {
        [JsonProperty("nhsNumber")]
        public string NhsNumber { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("dob")]
        public string Dob { get; set; }
        [JsonProperty("contactDetails")]
        public ContactDetails ContactDetails { get; set; }
        [JsonProperty("qualifiers")]
        public string Qualifiers { get; set; }
        [JsonProperty("selfReferralOccupation")]
        public string? SelfReferralOccupation { get; set; }
    }

    public class ContactDetails
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("landline")]
        public string Landline { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
