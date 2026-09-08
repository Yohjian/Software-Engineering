using Frontend.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Frontend.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ── HTTP client with JWT auth handler ────────────────────────────────────────
// Registers a single HttpClient that attaches the Bearer token before every
// request. No IHttpClientFactory needed — keeps the setup dependency-free.
builder.Services.AddScoped(sp =>
{
    var js = sp.GetRequiredService<IJSRuntime>();
    var handler = new JwtAuthorizationHandler(js)
    {
        InnerHandler = new HttpClientHandler(),
    };
    return new HttpClient(handler)
    {
        BaseAddress = new Uri("http://localhost:5000/api/"),
    };
});

// ── Authentication ────────────────────────────────────────────────────────────
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<CustomAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<CustomAuthStateProvider>());

// ── App services ──────────────────────────────────────────────────────────────
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TodoService>();

await builder.Build().RunAsync();
