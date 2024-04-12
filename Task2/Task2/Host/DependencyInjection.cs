using System.Reflection;
using CommunityToolkit.Diagnostics;
using Marten;
using Microsoft.OpenApi.Models;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.Marten;

namespace Host;

public static class DependencyInjection
{
    public static IApplicationBuilder UseHost(this WebApplication app)
    {
        app.MapWolverineEndpoints(opts => { opts.UseFluentValidationProblemDetailMiddleware(); });

        return app;
    }

    public static IServiceCollection AddMarten(this IServiceCollection services, IConfiguration configuration)
    {
        var dbSchemeName = configuration.GetSection("DbSettings")["DbSchemeName"];
        var connectionString =
            configuration.GetSection("DbSettings:ConnectionStrings")["MartenDb"]; //TODO: Change with new infrastructure
        Guard.IsNotNullOrEmpty(dbSchemeName, "Db scheme");
        Guard.IsNotNullOrEmpty(connectionString, "Connection string");

        services.AddMarten(opts =>
            {
                opts.Connection(connectionString);
                opts.DatabaseSchemaName = dbSchemeName;
            })
            .UseLightweightSessions()
            .IntegrateWithWolverine();
        return services;
    }

    public static IHostBuilder UseProjects(this IHostBuilder host, string[] assemblies)
    {
        host.UseWolverine(opts =>
        {
            foreach (var assembly in assemblies) opts.Discovery.IncludeAssembly(Assembly.Load(assembly));

            opts.Policies.AutoApplyTransactions();
            opts.Policies.UseDurableLocalQueues();
            opts.UseFluentValidation();
        });
        return host;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services, string swaggerApiName,
        string[] assemblies)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerApiName, Version = "v1" });

            foreach (var assembly in assemblies)
            {
                var assemblyXmlPath = Path.Combine(AppContext.BaseDirectory, $"{assembly}.xml");
                opt.IncludeXmlComments(assemblyXmlPath);
            }
        });
        return services;
    }
}