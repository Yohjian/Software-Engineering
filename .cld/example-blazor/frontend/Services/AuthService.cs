using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public class AuthService(HttpClient http, CustomAuthStateProvider authState)
{
    /// <summary>Returns (success, errorMessages).</summary>
    public async Task<(bool Success, List<string> Errors)> RegisterAsync(
        string email, string password)
    {
        var response = await http.PostAsJsonAsync("auth/register", new { email, password });
        if (response.IsSuccessStatusCode)
            return (true, []);

        // Identity returns an array of { code, description } on failure
        var errors = await response.Content
            .ReadFromJsonAsync<List<IdentityErrorDto>>();
        return (false, errors?.Select(e => e.Description).ToList()
                       ?? ["Registration failed."]);
    }

    /// <summary>Returns (success, errorMessage).</summary>
    public async Task<(bool Success, string? Error)> LoginAsync(
        string email, string password)
    {
        var response = await http.PostAsJsonAsync("auth/login", new { email, password });
        if (!response.IsSuccessStatusCode)
            return (false, "Invalid email or password.");

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result?.Token is null)
            return (false, "Unexpected server response.");

        await authState.MarkAuthenticatedAsync(result.Token);
        return (true, null);
    }

    public async Task LogoutAsync() =>
        await authState.MarkLoggedOutAsync();
}
