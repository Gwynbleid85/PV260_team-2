using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;

namespace ArkFunds.Emails;

public static class DependencyInjection
{
    public const string ArkHttpClientName = "ArkClient";

    public static IServiceCollection AddEmails(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMailer(configuration);
        return services;
    }

    public static IApplicationBuilder UseEmails(this WebApplication app)
    {
        app.MapWolverineEndpoints(opts => { opts.UseFluentValidationProblemDetailMiddleware(); });

        return app;
    }
}