using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Polly;
using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using System.Text;
using System.Net.Mime;
using System.Text.Json;

namespace NBS.Appointments.Service.Core.Services
{
    public class QflowService : IQflowService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQflowSessionManager _sessionManager;
        private readonly QflowOptions _options;

        private const string ApiSessionId = "ApiSessionId";

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
            var endpointUrl = $"{_options.BaseUrl}/svcCustomAppointment.svc/rest/availability";

            var response = await Execute(query, endpointUrl, HttpMethod.Get, null);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<SiteAvailabilityResponse[]>(responseBody);
        }

        public async Task<SiteSlotsResponse> GetSiteSlotAvailability(int siteId, DateTime date, string dose, string vaccineType, string externalReference)
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
            var endpointUrl = $"{_options.BaseUrl}/svcCustomAppointment.svc/rest/GetSiteDoseAvailability";

            var response = await Execute(query, endpointUrl, HttpMethod.Get, null);
            var responseBody = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<SiteSlotsResponse>(responseBody);
        }

        public async Task<ReserveSlotResponse> ReserveSlot(int calendarId, int startTime, int endTime, int lockDuration)
        {
            var request = new ReserveSlotRequestContent
            {
                CalendarId = calendarId,
                StartTime = startTime,
                EndTime = endTime,
                LockDuration = lockDuration,
                UserId = _options.UserId
            };

            var endpointUrl = $"{_options.BaseUrl}/svcCalendar.svc/rest/LockDynamicSlots";

            var response = await Execute(new Dictionary<string, string>(), endpointUrl, HttpMethod.Post, request);
            var responseBody = await response.Content.ReadAsStringAsync();

            var slotOrdinalNumber = int.Parse(responseBody);

            return new ReserveSlotResponse(slotOrdinalNumber);
        }

        private async Task<HttpResponseMessage> Execute(Dictionary<string, string> query, string endpointUrl, HttpMethod method, object? content)
        {
            using var client = _httpClientFactory.CreateClient();
            var context = new Dictionary<string, object>
            {
                {"SessionId", _sessionManager.GetSessionId()}
            };

            var requestMessage = new HttpRequestMessage(method, endpointUrl);

            var policy = GetRetryPolicy();
            return await policy.ExecuteAsync(async (context) =>
            {
                if (method == HttpMethod.Post)
                {
                    SetApiSessionId(content, context["SessionId"].ToString());
                    requestMessage.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json);
                }
                else
                {
                    query["apiSessionId"] = context["SessionId"].ToString();
                    requestMessage.RequestUri = new Uri(QueryHelpers.AddQueryString(endpointUrl, query));
                }

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

        private static void SetApiSessionId(object obj, string apiSessionId)
        {
            var basePayload = obj as BasePayload;

            if (basePayload != null)
            {
                basePayload.ApiSessionId = apiSessionId;
            }
        }
    }
}
