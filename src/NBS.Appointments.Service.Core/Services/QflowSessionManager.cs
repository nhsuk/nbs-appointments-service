using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NBS.Appointments.Service.Core
{
    public class QflowSessionManager : IQflowSessionManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly QflowOptions _options;
        private readonly IMutexRecordStore _sessionRecordStore;
        private readonly object _gate;
        private string _sessionId;
        private string _invalidSessionId;

        public QflowSessionManager(
            IOptions<QflowOptions> options, 
            IHttpClientFactory httpClientFactory,
            IMutexRecordStore sessionRecordStore)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _sessionRecordStore = sessionRecordStore ?? throw new ArgumentNullException(nameof(sessionRecordStore));
            _options = options.Value;
            _gate = new Object();
            _sessionId = String.Empty;
            _invalidSessionId = String.Empty;
        }

        private static readonly JsonSerializerOptions Config = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase),
            },
        };

        public string GetSessionId()
        {
            lock(_gate) // Prevent multiple threads on the same client colliding
            {
                if(String.IsNullOrEmpty(_sessionId)) // If we have a session id in memory we just use it
                {
                    // Acquire a mutex on a file containing the current session id - prevents multiple instances of the application signing in
                    using(var mutexFileAccess = _sessionRecordStore.Acquire("qflow_session.json"))
                    {
                        var remoteSessionId = mutexFileAccess.Read(); 
                        if(remoteSessionId != _invalidSessionId) // If the session id in the file is different then another instance must have signed in - so we can use this new session id
                        {
                            _sessionId = remoteSessionId;
                        }
                        else // The remote session id is the invalid one, so we need to sign in and replace it
                        {                            
                            var httpClient = _httpClientFactory.CreateClient();
                            var payload = new
                            {
                                UserName = _options.UserName,
                                Password = _options.Password,
                                ForceSignIn = true,
                                IpAddress = "127.0.0.1"
                            };
                            var content = new StringContent(JsonSerializer.Serialize(payload, Config));
                            var response = httpClient.PostAsync($"{_options.BaseUrl}/svcAppUser.svc/rest/FormsSignIn", content).GetAwaiter().GetResult();
                            if(response.StatusCode != System.Net.HttpStatusCode.OK)
                                throw new UnauthorizedAccessException();
                            _sessionId = response.Content.ReadAsStringAsync().GetAwaiter().GetResult().Replace("\"", "");
                            mutexFileAccess.Write(_sessionId);
                        }
                    }
                }
                return _sessionId;
            }
        }

        public void Invalidate(string sessionId)
        {
            lock(_gate)
            {
                if(_sessionId == sessionId)
                {
                    _invalidSessionId = sessionId; // track the invalid session id so we can check when getting a new session id
                    _sessionId = string.Empty;
                }
            }
        }
    }
}