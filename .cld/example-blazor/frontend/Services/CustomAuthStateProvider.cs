using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Frontend.Services;

public class CustomAuthStateProvider(IJSRuntime js) : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            var token = await js.InvokeAsync<string?>("localStorage.getItem", "token");
            if (string.IsNullOrWhiteSpace(token))
                return Anonymous;

            var claims = ParseClaimsFromJwt(token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
        catch
        {
            return Anonymous;
        }
    }

    public async Task MarkAuthenticatedAsync(string token)
    {
        await js.InvokeVoidAsync("localStorage.setItem", "token", token);
        var claims = ParseClaimsFromJwt(token);
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    public async Task MarkLoggedOutAsync()
    {
        await js.InvokeVoidAsync("localStorage.removeItem", "token");
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    // ── JWT parsing ───────────────────────────────────────────────────────────

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var kvps = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);
        return kvps?.Select(k => new Claim(k.Key, k.Value.ToString())) ?? [];
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        base64 = base64.Replace('-', '+').Replace('_', '/');
        return (base64.Length % 4) switch
        {
            2 => Convert.FromBase64String(base64 + "=="),
            3 => Convert.FromBase64String(base64 + "="),
            _ => Convert.FromBase64String(base64),
        };
    }
}
