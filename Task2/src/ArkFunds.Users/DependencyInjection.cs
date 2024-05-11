using ArkFunds.Users.Infrastructure;
using CommunityToolkit.Diagnostics;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;

namespace ArkFunds.Users;

public static class DependencyInjection
{
    public const string ArkHttpClientName = "ArkClient";

    public static IServiceCollection AddUsers(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSchemeName = configuration.GetSection("DbSettings")["DbSchemeName"];
        var connectionString = configuration.GetSection("DbSettings:ConnectionStrings")["MartenDb"];
        Guard.IsNotNullOrEmpty(dbSchemeName, "Db scheme");
        Guard.IsNotNullOrEmpty(connectionString, "Connection string");

        services.AddMartenStore<UsersStore>(opts =>
        {
            opts.Connection(connectionString);
            opts.DatabaseSchemaName = dbSchemeName;
        });
        
        return services;
    }

    public static IApplicationBuilder UseUsers(this WebApplication app)
    {
        app.MapWolverineEndpoints(opts => { opts.UseFluentValidationProblemDetailMiddleware(); });

        return app;
    }
}