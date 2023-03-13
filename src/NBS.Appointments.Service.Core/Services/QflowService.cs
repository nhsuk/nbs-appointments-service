using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Polly;
using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using Newtonsoft.Json;

namespace NBS.Appointments.Service.Core.Services
{
    public class QflowService : IQflowService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQflowSessionManager _sessionManager;
        private readonly QflowOptions _options;

        public QflowService(
            IOptions<QflowOptions> options,
            IHttpClientFactory httpClientFactory, 
            IQflowSessionManager sessionManager)
        {
            _options = options.Value;
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
        }

        public async Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate,
            string dose, string vaccine, string externalReference)
        {
            if(siteIds.Any() == false)
                throw new ArgumentException("At least one site must be provided", nameof(siteIds));

            if (String.IsNullOrEmpty(dose))
                throw new ArgumentException("A value must be provided", nameof(dose));

            if (String.IsNullOrEmpty(vaccine))
                throw new ArgumentException("A value must be provided", nameof(vaccine));

            int days = startDate.DaysBetween(endDate);
            if (days < 1)
                throw new ArgumentOutOfRangeException("The specified date range was not valid " + days);

            var query = new Dictionary<string, string>
            {
                { "Dose", dose },
                { "Vaccine", vaccine },
                { "SiteId", string.Join(",", siteIds)},
                { "StartDate", $"{startDate:yyyy-MM-dd}" },
                { "Days", days.ToString() },
                { "ExternalReference", externalReference ?? "" }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{_options.BaseUrl}/svcCustomAppointment.svc/rest/availability", query);

            var response = await Execute(query, endpointUrl, HttpMethod.Get);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<SiteAvailabilityResponse[]>(responseBody);
        }

        public async Task<AvailabilityByHourResponse> GetSiteSlotAvailability(int siteId, DateTime date, string dose, string vaccineType, string externalReference)
        {
            if (string.IsNullOrWhiteSpace(vaccineType))
                throw new ArgumentException($"A value for {nameof(vaccineType)} must be provided.");

            var query = new Dictionary<string, string>
            {
                { "Date", $"{date:yyyy-MM-dd}" },
                { "SiteId", siteId.ToString() },
                { "Dose", dose },
                { "VaccineType", vaccineType },
                { "ExternalReference", externalReference }
            };
            var endpointUrl = QueryHelpers.AddQueryString($"{_options.BaseUrl}/GetSiteDoseAvailability", query);

            var response = await Execute(query, endpointUrl, HttpMethod.Get);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<AvailabilityByHourResponse>(responseBody);
        }

        public async Task<ReserveSlotResponse> ReserveSlot(int calendarId, int startTime, int endTime, int lockDuration)
        {
            var request = new ReserveSlotRequestContent
            {
                CalendarId = calendarId,
                StartTime = startTime,
                EndTime = endTime,
                LockDuration = lockDuration
            };

            var endpointUrl = $"{_options.BaseUrl}/svcCalendar.svc/rest/LockDynamicSlots";
            var requestContent = new StringContent(JsonConvert.SerializeObject(request));

            var response = await Execute(new Dictionary<string, string>(), endpointUrl, HttpMethod.Post, requestContent);
            var responseBody = await response.Content.ReadAsStringAsync();

            _ = int.TryParse(responseBody, out var slotOrdinalNumber);

            var responseModel = new ReserveSlotResponse(slotOrdinalNumber);
            return responseModel;
        }

        private async Task<HttpResponseMessage> Execute(Dictionary<string, string> query, string endpointUrl, HttpMethod method, HttpContent? content = null)
        {
            using var client = _httpClientFactory.CreateClient();
            var context = new Dictionary<string, object>
            {
                {"SessionId", _sessionManager.GetSessionId()}
            };

            var requestMessage = new HttpRequestMessage(method, endpointUrl);

            if (content != null)
                requestMessage.Content = content;

            var policy = GetRetryPolicy();
            return await policy.ExecuteAsync(async (context) =>
            {
                query["apiSessionId"] = context["SessionId"].ToString();
                return await client.SendAsync(requestMessage);
            }, context);
        }

        private AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return Policy
                .HandleResult<HttpResponseMessage>(rsp => rsp.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                .RetryAsync(1, onRetry: (exception, retryCount, context) => {
                    Console.WriteLine("Session Invalid - retrying");
                    _sessionManager.Invalidate(context["SessionId"].ToString());
                    context["SessionId"] = _sessionManager.GetSessionId();
                });
        }
    }
}
