using Microsoft.Extensions.Options;
using NBS.Appointments.Service.Core.Interfaces;

namespace NBS.Appointments.Service.Core
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        private readonly DateTimeProviderOptions _options;

        public SystemDateTimeProvider(IOptions<DateTimeProviderOptions> options)
        {
            _options = options.Value;
        }

        public DateTime UtcNow => DateTime.UtcNow;

        public DateTime LocalNow => GetLocalNow();

        private DateTime GetLocalNow()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById(_options.TimeZone);
            var offset = tz.GetUtcOffset(DateTime.UtcNow);
            return DateTime.UtcNow + offset;
        }
    }

    public class RemoteDateTimeProvider : IDateTimeProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly DateTimeProviderOptions _options;

        public RemoteDateTimeProvider(IOptions<DateTimeProviderOptions> options, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public DateTime UtcNow => GetRemoteTime(DateTime.UtcNow, DateTimeKind.Utc);

        public DateTime LocalNow => GetRemoteTime(DateTime.Now, DateTimeKind.Local);

        private DateTime GetRemoteTime(DateTime now, DateTimeKind kind)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var response = httpClient.GetAsync(_options.Endpoint).GetAwaiter().GetResult().Content.ReadAsStringAsync().GetAwaiter().GetResult();
            switch (response)
            {
                case "@now":
                    return now;
                default:
                    return DateTime.SpecifyKind(DateTime.ParseExact(response, "yyyy-MM-dd HH:mm", null), kind);
            }
        }        
    }

    public class DateTimeProviderOptions
    {
        public string Type { get; set; }
        public string Endpoint { get; set; }
        public string TimeZone { get; set; }
    }
}
