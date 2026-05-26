using Distribuidora.WEB;
using Distribuidora.WEB.Auth;
using Distribuidora.WEB.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7156")
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<AuthService>();

// Auth
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationProviderJWT>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthenticationProviderJWT>(
    sp => sp.GetRequiredService<AuthenticationProviderJWT>());
builder.Services.AddScoped<ILoginService, AuthenticationProviderJWT>(
    sp => sp.GetRequiredService<AuthenticationProviderJWT>());

await builder.Build().RunAsync();
