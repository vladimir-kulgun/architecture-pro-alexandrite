using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var jaegerUrl = builder.Configuration["JAEGER_URL"] ?? "http://localhost:4317";

builder.Services.AddOpenTelemetry()
    .WithTracing(tpb => tpb
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CalculationService"))
        .AddAspNetCoreInstrumentation()
        .AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerUrl)));

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/calculate/{id}", (int id) => Results.Ok(new { Result = id * 1.5, Tax = 0.2 }));

app.Run("http://0.0.0.0:5002");