using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Interfaces;

namespace NBS.Appointments.Service.Core
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;        
    }

    public class RemoteDateTimeProvider : IDateTimeProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RemoteDateTimeProviderOptions _options;

        public RemoteDateTimeProvider(IOptions<RemoteDateTimeProviderOptions> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public DateTime UtcNow
        {
            get
            {
                var httpClient = _httpClientFactory.CreateClient();                
                var response = httpClient.GetAsync(_options.Endpoint).GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();
                switch (response)
                {
                    case "@now":
                        return DateTime.Now;
                    case "@utc":
                        return DateTime.UtcNow;
                    default:
                        return DateTime.SpecifyKind(DateTime.ParseExact(response, "yyyy-MM-dd HH:mm", null), DateTimeKind.Utc);
                }
            }
        }        
    }

    public class RemoteDateTimeProviderOptions
    {
        public string Endpoint { get; set; }
    }
}
