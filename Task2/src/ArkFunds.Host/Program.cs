using ArkFunds.Emails;
using ArkFunds.Host;
using ArkFunds.Reports;
using CommunityToolkit.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMarten(builder.Configuration);

var assemblies = builder.Configuration.GetSection("Assemblies").Get<string[]>();
Guard.IsNotNull(assemblies, "Program assemblies");

builder.Services.AddSwagger("PV260 API", assemblies[..1]);

builder.Services.AddReports(builder.Configuration);
builder.Services.AddEmails(builder.Configuration);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHost();

app.UseHttpsRedirection();

app.Run();