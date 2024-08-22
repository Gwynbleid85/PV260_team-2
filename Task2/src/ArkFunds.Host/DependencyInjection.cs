using System.Reflection;
using CommunityToolkit.Diagnostics;
using Marten;
using Microsoft.OpenApi.Models;
using Wolverine;
using Wolverine.FluentValidation;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;
using Wolverine.Marten;

namespace ArkFunds.Host;

public static class DependencyInjection
{
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
            
            opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                        TokenUrl = new Uri("https://localhost:5001/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            {"ArkFundsAPI", "API - full access"}
                        }
                    },
                }
            });
            
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { "ArkFundsAPI" }
                }
            });
        });
        return services;
    }
}