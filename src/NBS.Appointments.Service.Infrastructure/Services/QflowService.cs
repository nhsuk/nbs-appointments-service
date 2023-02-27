using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Infrastructure.Interfaces.Services;

namespace NBS.Appointments.Service.Infrastructure.Services
{
    public class QflowService : IQflowService
    {
        public SiteAvailabilityResponse GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate, string type,
            string dose, string vaccine, string externalReference)
        {
            throw new NotImplementedException();
        }
    }
}
