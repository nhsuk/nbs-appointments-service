using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CustomerDto
    {
        [JsonProperty("PersonalId")]
        public string NHSNumber { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("CustomerLevelId")]
        public int CustomerLevelId { get; set; }

        [JsonProperty("CustomerIdTypeId")]
        public int CustomerIdTypeId { get; set; }

        [JsonProperty("EMail")]
        public string Email { get; set; }

        [JsonProperty("TelNumber1")]
        public string TelNumber1 { get; set; }

        [JsonProperty("TelNumber2")]
        public string TelNumber2 { get; set; }

        [JsonProperty("Notes")]
        public string Notes { get; set; }

        [JsonProperty("LanguageCode")]
        public string LanguageCode { get; set; }

        [JsonProperty("DOB")]
        public string Dob { get; set; }

        [JsonProperty("Sex")]
        public int Sex { get; set; }

        [JsonProperty("PictureAttachmentId")]
        public int PictureAttachmentId { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonIgnore]
        public string CustomProperties { get; set; }

        [JsonProperty("CustomerLevelName")]
        public string CustomerLevelName { get; set; }

        [JsonProperty("CustomerIdTypeName")]
        public string CustomerIdTypeName { get; set; }

        [JsonProperty("IsCustomerGroup")]
        public bool IsCustomerGroup { get; set; }

        [JsonProperty("IsMemberOfGroups")]
        public bool IsMemberOfGroups { get; set; }

        [JsonProperty("NotificationConsent")]
        public int NotificationConsent { get; set; }

        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Active")]
        public bool Active { get; set; }

        [JsonProperty("ExtRef")]
        public string ExtRef { get; set; }

        [JsonProperty("PartOfQapp")]
        public bool PartOfQapp { get; set; }
    }
}
