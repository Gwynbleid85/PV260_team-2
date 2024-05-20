using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ArkFunds.App.Web.Client;
using ArkFunds.App.Web.Client.Api;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

builder.Services
    .AddHttpClient()
    .AddTransient<IReportsClient, ReportsClient>(sp =>
        new ReportsClient("https://localhost:5019", sp.GetService<HttpClient>()))
    .AddTransient<IUsersClient, UsersClient>(sp =>
        new UsersClient("https://localhost:5019", sp.GetService<HttpClient>()));

await builder.Build().RunAsync();
