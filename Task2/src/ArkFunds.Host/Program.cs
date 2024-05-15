using ArkFunds.Host;
using ArkFunds.Reports;
using ArkFunds.Users;
using CommunityToolkit.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMarten(builder.Configuration);

var assemblies = builder.Configuration.GetSection("Assemblies").Get<string[]>();
Guard.IsNotNull(assemblies, "Program assemblies");

builder.Services.AddSwagger("PV260 API", assemblies);

builder.Services.AddReports(builder.Configuration);
builder.Services.AddUsers(builder.Configuration);


builder.Host.UseProjects(assemblies);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHost();

app.UseHttpsRedirection();


app.Run();