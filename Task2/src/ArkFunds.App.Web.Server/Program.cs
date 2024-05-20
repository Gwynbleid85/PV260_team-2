using ArkFunds.App.Web.Client.Api;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ArkFunds.App.Web.Server;
using ArkFunds.App.Web.Server.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("oidc")
    .AddOpenIdConnect("oidc", oidcOptions =>
    {
        oidcOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        oidcOptions.Scope.Add("ArkFundsAPI");
        oidcOptions.Authority = "https://localhost:5001";
        oidcOptions.ClientId = "blazor_web_app";
        oidcOptions.ClientSecret = "d422a063-ca8f-4104-a3c4-60fc798519c5";
        oidcOptions.ResponseType = OpenIdConnectResponseType.Code;

        oidcOptions.MapInboundClaims = false;
        oidcOptions.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

builder.Services.ConfigureCookieOidcRefresh(CookieAuthenticationDefaults.AuthenticationScheme, "oidc");

builder.Services.AddAuthorization();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped<AuthenticationStateProvider, PersistingAuthenticationStateProvider>();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddHttpClient()
    .AddTransient<IReportsClient, ReportsClient>(sp =>
        new ReportsClient(builder.Configuration.GetConnectionString("ApiBaseUrl"), sp.GetService<HttpClient>()))
    .AddTransient<IUsersClient, UsersClient>(sp =>
        new UsersClient(builder.Configuration.GetConnectionString("ApiBaseUrl"), sp.GetService<HttpClient>()));

builder.Services.AddCors(policy =>
{
    policy.AddPolicy("_myAllowSpecificOrigins", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ArkFunds.App.Web.Client._Imports).Assembly);

app.MapGroup("/authentication").MapLoginAndLogout();

app.UseCors("_myAllowSpecificOrigins");

app.Run();
