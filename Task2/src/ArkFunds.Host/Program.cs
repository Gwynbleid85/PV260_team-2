using ArkFunds.Emails;
using ArkFunds.Host;
using ArkFunds.Reports;
using ArkFunds.Users;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using CommunityToolkit.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Wolverine.Http;
using Wolverine.Http.FluentValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMarten(builder.Configuration);

var assemblies = builder.Configuration.GetSection("Assemblies").Get<string[]>();
Guard.IsNotNull(assemblies, "Program assemblies");

builder.Services.AddSwagger("PV260 API", assemblies[..1]);

builder.Services.AddReports(builder.Configuration);
builder.Services.AddEmails(builder.Configuration);
builder.Services.AddUsers(builder.Configuration);

builder.Host.UseProjects(assemblies);

builder.Services.AddOpenTelemetry().ConfigureResource(r => r.AddService("OtelWebApi"))
    .WithTracing(t =>
        t.AddSource("Wolverine")
            .AddHttpClientInstrumentation()
            .AddAspNetCoreInstrumentation()
            .AddJaegerExporter(
                options =>
                {
                    options.AgentHost = "localhost";
                    options.AgentPort = 6831;
                }
            )
    );

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(o =>
        o.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options => {
        options.Authority = "https://localhost:5001";
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false, // TODO: Validate 
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "ArkFundsAPI");
    });
});

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        opts.OAuthUsePkce();
    });
}

app.MapWolverineEndpoints(opts =>
{
    opts.UseFluentValidationProblemDetailMiddleware();
    opts.ConfigureEndpoints(e => e.RequireAuthorization("ApiScope"));
});

// app.UseReports();
// app.UseEmails();
// app.UseUsers();
app.UseReports();

app.UseHttpsRedirection();


app.Run();