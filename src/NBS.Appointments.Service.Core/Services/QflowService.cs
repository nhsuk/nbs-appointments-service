using NBS.Appointments.Service.Dtos.Qflow;
using NBS.Appointments.Service.Core.Interfaces.Services;
using Microsoft.AspNetCore.WebUtilities;
using Polly;
using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Dtos.Qflow;
using System.Text;
using System.Net.Mime;
using System.Text.Json;
using NBS.Appointments.Service.Core.Dtos;
using Microsoft.Extensions.Logging;

namespace NBS.Appointments.Service.Core.Services
{
    public class QflowService : IQflowService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IQflowSessionManager _sessionManager;
        private readonly QflowOptions _options;
        private readonly ILogger<QflowService> _logger;

        public QflowService(
            IOptions<QflowOptions> options,
            IHttpClientFactory httpClientFactory, 
            IQflowSessionManager sessionManager,
            ILogger<QflowService> logger)
        {
            _options = options.Value;

            _httpClientFactory = httpClientFactory
                ?? throw new ArgumentNullException(nameof(httpClientFactory));

            _sessionManager = sessionManager
                ?? throw new ArgumentNullException(nameof(sessionManager));

            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<SiteAvailabilityResponse[]> GetSiteAvailability(IEnumerable<string> siteIds, DateTime startDate, DateTime endDate,
            string dose, string vaccine, string externalReference)
        {
            if(siteIds.Any() == false)
                throw new ArgumentException("At least one site must be provided", nameof(siteIds));

            if (string.IsNullOrEmpty(dose))
                throw new ArgumentException("A value must be provided", nameof(dose));

            if (string.IsNullOrEmpty(vaccine))
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

            try
            {
                return JsonSerializer.Deserialize<SiteAvailabilityResponse[]>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown when attempting to deserialize qflow site availability response. Message: {@Message}, Response body: {@ResponseBody}, StatusCode: {@StatusCode}",
                    ex.Message, responseBody, response.StatusCode);
                throw;
            }
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

            try
            {
                return JsonSerializer.Deserialize<SiteSlotsResponse>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown when attempting to deserialize qflow site slot availability response. Message: {@Message}, Response body: {@ResponseBody}, StatusCode: {@StatusCode}",
                    ex.Message, responseBody, response.StatusCode);
                throw;
            }
        }

        public async Task<ApiResult<ReserveSlotResponse>> ReserveSlot(int calendarId, int startTime, int endTime, int lockDuration)
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

            var result = new ApiResult<ReserveSlotResponse>
            {
                StatusCode = response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                var slotOrdinalNumber = int.Parse(responseBody);

                result.ResponseData = new ReserveSlotResponse(slotOrdinalNumber);
                return result;
            }

            _logger.LogWarning("Reserve slot returned non-successful status code. Status code: {@StatusCode}. Response body: {@ResponseBody}. Payload: {@Payload}",
                response.StatusCode, responseBody, request);

            return result;
        }

        public async Task<ApiResult<BookAppointmentResponse>> BookAppointment(int serviceId, DateTime dateAndTime, int customerId, int appointmentTypeId,
            int slotOrdinalNumber, int calendarId, Dictionary<string, string>? customProperties)
        {
            var payload = new BookAppointmentPayload
            {
                AppointmentTypeId = appointmentTypeId,
                CalendarId = calendarId,
                CustomerId = customerId,
                CustomProperties = customProperties,
                DateAndTime = $"{dateAndTime:yyyy-MM-dd HH:mm:ss}",
                ServiceId = serviceId,
                SlotOrdinalNumber = slotOrdinalNumber,
            };

            var endpointUrl = $"{_options.BaseUrl}/svcService.svc/rest/SetAppointment6";

            var response = await Execute(new Dictionary<string, string>(), endpointUrl, HttpMethod.Post, payload);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = new ApiResult<BookAppointmentResponse>
            {
                StatusCode = response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                result.ResponseData = JsonSerializer.Deserialize<BookAppointmentResponse>(responseBody);
                return result;
            }

            _logger.LogWarning("Book appointment returned non-successful status code. Status code: {@StatusCode}. Response body: {@ResponseBody}. Payload: {@Payload}",
                response.StatusCode, responseBody, payload);

            return result;
        }

        public async Task<ApiResult<CustomerDto>> CreateOrUpdateCustomer(string firstName, string surname, string nhsNumber, string dob, string? email,
            string? phoneNumber, string? landline, string selfReferralOccupation)
        {
            var payload = new CreateCustomerPayload
            {
                Customer = new Customer
                {
                    DoB = $"{dob:d}",
                    Email = email,
                    FirstName = firstName,
                    LastName = surname,
                    NHSNumber = nhsNumber,
                    SelfReferralOccupation = selfReferralOccupation,
                    TelNumber1 = phoneNumber,
                    TelNumber2 = landline,
                    NotificationConsent = !string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(phoneNumber) && !string.IsNullOrEmpty(landline)
                }
            };

            var endpointUrl = $"{_options.BaseUrl}/svcCustomCustomer.svc/rest/Create";

            var response = await Execute(new Dictionary<string, string>(), endpointUrl, HttpMethod.Post, payload);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = new ApiResult<CustomerDto>
            {
                StatusCode = response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                result.ResponseData = JsonSerializer.Deserialize<CustomerDto>(responseBody);
                return result;
            }

            _logger.LogWarning("Create or update customer returned non-successful status code. Status code: {@StatusCode}. Response body: {@ResponseBody}. Payload: {@Payload}",
                response.StatusCode, responseBody, payload);

            return result;
        }

        public async Task<IList<AppointmentResponse>> GetAllCustomerAppointments(long qflowCustomerId)
        {
            var query = new Dictionary<string, string>
            {
                { "QflowCustomerId", qflowCustomerId.ToString() }
            };
            var endpointUrl = $"{_options.BaseUrl}/svcCustomAppointment.svc/rest/GetAllCustomerAppointments";

            var response = await Execute(query, endpointUrl, HttpMethod.Get, null);
            var responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                return JsonSerializer.Deserialize<List<AppointmentResponse>>(responseBody);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown when attempting to deserialize customer appointments response. Message: {@Message}, Response body: {@ResponseBody}, StatusCode: {@StatusCode}",
                    ex.Message, responseBody, response.StatusCode);
                throw;
            }
        }

        public async Task<ApiResult<CancelBookingResponse>> CancelAppointment(int processId, int cancelationReasonId, int treatmentPlanCancelationMethod)
        {
            var payload = new CancelBookingPayload
            {
                ProcessId = processId,
                CancellationReasonId = cancelationReasonId,
                TreatmentPlanCancellationMethod = treatmentPlanCancelationMethod,
                // TODO: These are the same throughout existing NBS, should we add them to the cancelation descriptor?
                CancellationType = 0,
                CustomerTreatmentPlanId = 0,
                ParentCaseId = 0,
                RemoveWaitingListRequest = false
            };

            var endpointUrl = $"{_options.BaseUrl}/svcProcess.svc/rest/CancelAppointment1";
             var response = await Execute(new Dictionary<string, string>(), endpointUrl, HttpMethod.Post, payload);
             var responseBody = await response.Content.ReadAsStringAsync();

             var result = new ApiResult<CancelBookingResponse>
             {
                 StatusCode = response.StatusCode
             };

             if (response.IsSuccessStatusCode)
             {
                 result.ResponseData = JsonSerializer.Deserialize<CancelBookingResponse>(responseBody);
                 return result;
             }

            _logger.LogWarning("Cancel appointment returned non-successful status code. Status code: {@StatusCode}. Response body: {@ResponseBody}. Payload: {@Payload}",
               response.StatusCode, responseBody, payload);

            return result;
        }

        public async Task<ApiResult<CustomerDto>> GetCustomerByNhsNumber(string nhsNumber)
        {
            var query = new Dictionary<string, string>
            {
                { "nhsNumber", nhsNumber.ToString() }
            };

            var endpointUrl = $"{_options.BaseUrl}/svcCustomNHSNumber.svc/rest/GetByNHSNumber";
            var response = await Execute(query, endpointUrl, HttpMethod.Get, null);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = new ApiResult<CustomerDto>
            {
                StatusCode = response.StatusCode
            };

            if (string.IsNullOrEmpty(responseBody))
                return result;

            if (response.IsSuccessStatusCode)
            {
                result.ResponseData = JsonSerializer.Deserialize<CustomerDto>(responseBody);
                return result;
            }

            _logger.LogWarning("Get customer by NHS number returned non-successful status code. Status code: {@StatusCode}. Response body: {@ResponseBody}. NHS Number: {@NhsNumber}",
               response.StatusCode, responseBody, nhsNumber);

            return result;
        }

        public async Task<ApiResult<RescheduleAppointmentResponse>> RescheduleAppointment(int serviceId, DateTime startDateTime, int appointmentTypeId, long processId)
        {
            var payload = new RescheduleAppointmentPayload
            {
                AppointmentTypeId = appointmentTypeId,
                CancelationReasonId = _options.DefaultRescheduleReasonId,
                OriginalProcessId = processId,
                ServiceId = serviceId,
                DateAndTime = startDateTime
            };

            var endpointUrl = $"{_options.BaseUrl}/svcProcess.svc/rest/RescheduleAppointment";
            var response = await Execute(new Dictionary<string, string>(), endpointUrl, HttpMethod.Post, payload);
            var responseBody = await response.Content.ReadAsStringAsync();

            var result = new ApiResult<RescheduleAppointmentResponse>
            {
                StatusCode = response.StatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                result.ResponseData = JsonSerializer.Deserialize<RescheduleAppointmentResponse>(responseBody);
                return result;
            }

            _logger.LogWarning("Get customer by NHS number returned non-successful status code. Status code: {@StatusCode}. Response body: {@ResponseBody}. Payload: {@Payload}",
               response.StatusCode, responseBody, payload);

            return result;
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
