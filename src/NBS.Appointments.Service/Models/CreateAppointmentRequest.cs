using Newtonsoft.Json;

namespace NBS.Appointments.Service.Models
{
    public class CreateAppointmentRequest
    {
        [JsonProperty("slot")]
        public string Slot { get; set; }
        [JsonProperty("nhsNumber")]
        public string NhsNumber { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        public ContactDetails ContactDetails { get; set; }
        [JsonProperty("qualifiers")]
        public string Qualifiers { get; set; }
        [JsonProperty("properties")]
        public string Properties { get; set; }
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
