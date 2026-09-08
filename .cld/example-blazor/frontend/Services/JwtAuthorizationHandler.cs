using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace Frontend.Services;

/// <summary>
/// Reads the JWT token from localStorage before every outgoing request
/// and attaches it as a Bearer Authorization header.
/// </summary>
public class JwtAuthorizationHandler(IJSRuntime js) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await js.InvokeAsync<string?>("localStorage.getItem", "token");
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }
}
