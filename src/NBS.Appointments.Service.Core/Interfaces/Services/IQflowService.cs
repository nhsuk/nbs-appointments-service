using NBS.Appointments.Service.Core.Dtos.Qflow;
using NBS.Appointments.Service.Dtos.Qflow;

namespace NBS.Appointments.Service.Core.Interfaces.Services
{
    public interface IQflowService
    {
        public Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate, string dose, string vaccine, string externalReference);

        public Task<AvailabilityByHourResponse> GetSiteSlotAvailabilityAsync(int siteId, DateTime date, string appointmentType);
    }
}
