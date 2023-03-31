using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CustomerDto
    {
        [JsonPropertyName("PersonalId")]
        public string NHSNumber { get; set; }

        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("LastName")]
        public string LastName { get; set; }

        [JsonPropertyName("CustomerLevelId")]
        public int CustomerLevelId { get; set; }

        [JsonPropertyName("CustomerIdTypeId")]
        public int CustomerIdTypeId { get; set; }

        [JsonPropertyName("EMail")]
        public string Email { get; set; }

        [JsonPropertyName("TelNumber1")]
        public string TelNumber1 { get; set; }

        [JsonPropertyName("TelNumber2")]
        public string TelNumber2 { get; set; }

        [JsonPropertyName("Notes")]
        public string Notes { get; set; }

        [JsonPropertyName("LanguageCode")]
        public string LanguageCode { get; set; }

        [JsonPropertyName("DOB")]
        public string Dob { get; set; }

        [JsonPropertyName("Sex")]
        public int Sex { get; set; }

        [JsonPropertyName("PictureAttachmentId")]
        public int PictureAttachmentId { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string CustomProperties { get; set; }

        [JsonPropertyName("CustomerLevelName")]
        public string CustomerLevelName { get; set; }

        [JsonPropertyName("CustomerIdTypeName")]
        public string CustomerIdTypeName { get; set; }

        [JsonPropertyName("IsCustomerGroup")]
        public bool IsCustomerGroup { get; set; }

        [JsonPropertyName("IsMemberOfGroups")]
        public bool IsMemberOfGroups { get; set; }

        [JsonPropertyName("NotificationConsent")]
        public int NotificationConsent { get; set; }

        [JsonPropertyName("Id")]
        public int Id { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }

        [JsonPropertyName("Active")]
        public bool Active { get; set; }

        [JsonPropertyName("ExtRef")]
        public string ExtRef { get; set; }

        [JsonPropertyName("PartOfQapp")]
        public bool PartOfQapp { get; set; }
    }
}
