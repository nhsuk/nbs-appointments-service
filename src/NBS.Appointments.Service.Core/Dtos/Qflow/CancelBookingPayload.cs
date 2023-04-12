using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core.Dtos.Qflow
{
    public class CancelBookingPayload : BasePayload
    {
        [JsonPropertyName("processId")]
        public int ProcessId { get; set; }

        [JsonPropertyName("cancelationReasonId")]
        public int CancellationReasonId { get; set; }

        [JsonPropertyName("cancelationType")]
        public int CancellationType { get; set; }

        [JsonPropertyName("parentCaseId")]
        public int ParentCaseId { get; set; }

        [JsonPropertyName("treatmentPlanCancelationMethod")]
        public int TreatmentPlanCancellationMethod { get; set; }

        [JsonPropertyName("removeWaitingListRequest")]
        public bool RemoveWaitingListRequest { get; set; }

        [JsonPropertyName("customerTreatmentPlaneId")]
        public int CustomerTreatmentPlanId { get; set; }
    }
}
