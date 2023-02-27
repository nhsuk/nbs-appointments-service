using Microsoft.Extensions.Options;

namespace NBS.Appointments.Service.Core
{
    public interface IQflowSessionManager
    {
        Task<string> GetSessionId();
        void Invalidate(string sessionId);
    }

    public class QflowSessionManager : IQflowSessionManager
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly QflowOptions _options;
        private readonly object _gate;
        private string _sessionId;

        public QflowSessionManager(IOptions<QflowOptions> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _options = options.Value;
            _gate = new Object();
            _sessionId = String.Empty;
        }

        public Task<string> GetSessionId()
        {
            lock(_gate)
            {
                if(String.IsNullOrEmpty(_sessionId))
                {
                    // TODO: Sign in and get a new session id
                    _sessionId = Guid.NewGuid().ToString();
                }
                return Task.FromResult(_sessionId);
            }
        }

        public void Invalidate(string sessionId)
        {
            lock(_gate)
            {
                if(_sessionId == sessionId)
                    _sessionId = string.Empty;
            }
        }
    }

    public class QflowOptions
    {
        public string BaseUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}