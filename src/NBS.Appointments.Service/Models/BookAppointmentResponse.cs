using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Core.Dtos.Qflow.Descriptors;
using NBS.Appointments.Service.Core.Helpers;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Models
{
    public class BookedAppointmentResponse
    {
        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        public static BookedAppointmentResponse FromQflowResponse(int qflowCustomerId, BookAppointmentResponse qflowResponse) => new BookedAppointmentResponse
        {
            Ref = DescriptorConverter.ToString(new QFlowAppointmentReferenceDescriptor
            {
                CustomerId= qflowCustomerId,
                CaseId = qflowResponse.CaseId,
                ProcessId = qflowResponse.ProcessId
            })
        };
    }
}
