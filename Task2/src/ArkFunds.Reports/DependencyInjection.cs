using ArkFunds.Reports.Application.ServiceInterfaces;
using ArkFunds.Reports.Core.Enums;
using ArkFunds.Reports.Infrastructure;
using CommunityToolkit.Diagnostics;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;

namespace ArkFunds.Reports;

using TimeProvider = Infrastructure.TimeProvider;

public static class DependencyInjection
{
    public const string ArkHttpClientName = "ArkClient";

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
        
        services.AddHttpClient(ArkHttpClientName, client =>
        {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
        });

        // Add custom services
        services
            .AddSingleton<ITimeProvider, TimeProvider>()
            .AddSingleton<IReportGenerator, ReportGenerator>();
        
        var sourceType = configuration.GetSection("ReportsSettings").GetValue<ReportSourceType?>("SourceType");
        Guard.IsNotNull(sourceType, "Report source type");
        
        if (sourceType == ReportSourceType.Csv)
        {
            services
                .AddSingleton<IReportParser, CsvReportParser>()
                .AddSingleton<IReportReader, CsvReportReader>();
        }

        return services;
    }

    public static IApplicationBuilder UseReports(this WebApplication app)
    {
        app.MapWolverineEndpoints(opts => { opts.UseFluentValidationProblemDetailMiddleware(); });

        return app;
    }
}