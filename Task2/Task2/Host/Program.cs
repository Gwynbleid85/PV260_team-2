using Host;
using Reports;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddMarten(builder.Configuration);

string[] assemblies = ["Reports"];

builder.Services.AddSwagger("PV260 API", assemblies);

builder.Services.AddReports(builder.Configuration);

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