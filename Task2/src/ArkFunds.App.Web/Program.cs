using ArkFunds.App.Web.Api;
using ArkFunds.App.Web.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

// Add api clients
builder.Services
    .AddHttpClient()
    .AddTransient<IReportsClient, ReportsClient>(sp =>
        new ReportsClient(builder.Configuration.GetConnectionString("ApiBaseUrl"), sp.GetService<HttpClient>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
