using CommunityToolkit.Diagnostics;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reports.Application.ServiceInterfaces;
using Reports.Infrastructure;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using TimeProvider = Reports.Infrastructure.TimeProvider;

namespace Reports;

public static class DependencyInjection
{
    public static IServiceCollection AddReports(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSchemeName = configuration.GetSection("DbSettings")["DbSchemeName"];
        var connectionString = configuration.GetSection("DbSettings:ConnectionStrings")["MartenDb"];
        Guard.IsNotNullOrEmpty(dbSchemeName, "Db scheme");
        Guard.IsNotNullOrEmpty(connectionString, "Connection string");

        services.AddMartenStore<ReportsStore>(opts =>
        {
            opts.Connection(connectionString);
            opts.DatabaseSchemaName = dbSchemeName;
        });

        // Add custom services
        services.AddTimeProvider();
        services.AddReportGenerator();

        return services;
    }

    public static IApplicationBuilder UseReports(this WebApplication app)
    {
        app.MapWolverineEndpoints(opts => { opts.UseFluentValidationProblemDetailMiddleware(); });

        return app;
    }

    private static void AddTimeProvider(this IServiceCollection services)
    {
        services.AddSingleton<ITimeProvider>(sp => new TimeProvider());
    }

    private static void AddReportGenerator(this IServiceCollection services)
    {
        services.AddSingleton<IReportGenerator>(sp => new ReportGenerator());
    }
}