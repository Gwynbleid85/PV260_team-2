using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Infrastructure;
using CommunityToolkit.Diagnostics;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using TimeProvider = ArkFunds.Reports.Infrastructure.TimeProvider;

namespace ArkFunds.Reports;

using TimeProvider = Infrastructure.TimeProvider;

public static class DependencyInjection
{
    // TODO: move to a configuration class
    public const string ArkClientName = "ArkClient";

    public static IServiceCollection AddReports(this IServiceCollection services, IConfiguration configuration)
    {
        var reportsBaseUrl = configuration.GetSection("ReportsSettings")["BaseUrl"];
        var dbSchemeName = configuration.GetSection("DbSettings")["DbSchemeName"];
        var connectionString = configuration.GetSection("DbSettings:ConnectionStrings")["MartenDb"];
        Guard.IsNotNullOrEmpty(reportsBaseUrl, "Reports base url");
        Guard.IsNotNullOrEmpty(dbSchemeName, "Db scheme");
        Guard.IsNotNullOrEmpty(connectionString, "Connection string");

        services.AddMartenStore<ReportsStore>(opts =>
        {
            opts.Connection(connectionString);
            opts.DatabaseSchemaName = dbSchemeName;
        });
        
        services.AddHttpClient(ArkClientName, client =>
        {
            // TODO: move this to configuration
            client.BaseAddress = new Uri(reportsBaseUrl);
            // TODO: simplify/to configuration ?
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
        });

        // Add custom services
        services
            .AddSingleton<ITimeProvider, TimeProvider>()
            .AddSingleton<IReportGenerator, CsvReportGenerator>();

        return services;
    }

    public static IApplicationBuilder UseReports(this WebApplication app)
    {
        app.MapWolverineEndpoints(opts => { opts.UseFluentValidationProblemDetailMiddleware(); });

        return app;
    }
}