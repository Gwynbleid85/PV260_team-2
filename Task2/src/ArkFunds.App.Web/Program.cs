using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ArkFunds.App.Web;
using ArkFunds.App.Web.Api;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler(serviceProvider
        => serviceProvider?.GetService<AuthorizationMessageHandler>()
            ?.ConfigureHandler(
                authorizedUrls: new[] { apiBaseUrl },
                scopes: new[] { "ArkFundsAPI" }));

builder.Services.AddScoped<HttpClient>(serviceProvider => serviceProvider.GetService<IHttpClientFactory>().CreateClient("api"));

builder.Services.AddScoped<IReportsClient, ReportsClient>(provider => new ReportsClient(apiBaseUrl, provider.GetService<HttpClient>()));
builder.Services.AddScoped<IUsersClient, UsersClient>(provider => new UsersClient(apiBaseUrl, provider.GetService<HttpClient>()));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("IdentityServer", options.ProviderOptions);
    var configurationSection = builder.Configuration.GetSection("IdentityServer");
    var authority = configurationSection["Authority"];

    options.ProviderOptions.DefaultScopes.Add("ArkFundsAPI");
});

await builder.Build().RunAsync();