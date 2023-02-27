using NBS.Appointments.Service.Dtos.Qflow;

namespace NBS.Appointments.Service.Infrastructure.Interfaces.Services
{
    public interface IQflowService
    {
        public SiteAvailabilityResponse GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate, string type, string dose, string vaccine, string externalReference);
    }
}
