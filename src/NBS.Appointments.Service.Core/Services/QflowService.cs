using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;

namespace NBS.Appointments.Service.Core.Services
{
    public class QflowService : IQflowService
    {
        private const string QflowUrl = "http://mock-api";

        private readonly IHttpClientFactory _httpClientFactory;

        public QflowService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate,
            string dose, string vaccine, string externalReference)
        {
            var query = new Dictionary<string, string>
            {
                { "Dose", dose.ToString() },
                { "Vaccine", vaccine },
                { "SiteId", string.Join(",", siteIds)},
                { "StartDate", $"{startDate:yyyy-MM-dd}" },
                { "Days", endDate.DaysBetween(startDate).ToString() },
                { "ExternalReference", externalReference ?? "" }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{QflowUrl}/svcCustomAppointment.svc/rest/", query);
            
            using var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(endpointUrl);

            var responseBody = await response.Content.ReadAsStringAsync();
            Console.WriteLine(responseBody);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<SiteAvailabilityResponse[]>(responseBody);
        }
    }    
}
