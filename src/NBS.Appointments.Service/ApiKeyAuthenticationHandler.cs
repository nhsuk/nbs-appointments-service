using System;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace NBS.Appointments.Service;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
{
    public string ApiKey { get; set; }
}

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{
    private const string API_KEY_HEADER_NAME = "nbs-api-key";    
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionsMonitor<ApiKeyAuthenticationOptions> _options;
    public ApiKeyAuthenticationHandler(
        IHttpContextAccessor httpContextAccessor,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        ISystemClock systemClock,
        IOptionsMonitor<ApiKeyAuthenticationOptions> options)
        : base(options, loggerFactory, encoder, systemClock)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options;
    }
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var clientApiKey = GetSubmittedApiKey(httpContext);

        if (!IsApiKeyValid(_options.CurrentValue.ApiKey, clientApiKey))
        {
            return Task.FromResult(AuthenticateResult.Fail(""));
        }

        var claimsIdentity = new ClaimsIdentity("Service");
        var principal = new ClaimsPrincipal(claimsIdentity);
        var ticket = new AuthenticationTicket(principal, SchemeName);
        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    protected virtual string SchemeName => Scheme.Name;

    private static string GetSubmittedApiKey(HttpContext context)
    {
        return context.Request.Headers[API_KEY_HEADER_NAME];
    }    

    private static bool IsApiKeyValid(string apiKey, string submittedApiKey)
    {
        if (string.IsNullOrEmpty(submittedApiKey)) return false;

        var apiKeySpan = MemoryMarshal.Cast<char, byte>(apiKey.AsSpan());

        var submittedApiKeySpan = MemoryMarshal.Cast<char, byte>(submittedApiKey.AsSpan());

        return CryptographicOperations.FixedTimeEquals(apiKeySpan, submittedApiKeySpan);
    }
}