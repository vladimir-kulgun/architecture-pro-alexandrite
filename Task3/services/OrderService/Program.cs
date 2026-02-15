using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Чтение конфигов из переменных окружения (ConfigMap)
var jaegerUrl = builder.Configuration["JAEGER_URL"] ?? "http://localhost:4317";
var calcServiceUrl = builder.Configuration["CALCULATION_SERVICE_URL"] ?? "http://localhost:5002";

builder.Services.AddOpenTelemetry()
    .WithTracing(tpb => tpb
        .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OrderService"))
        .AddAspNetCoreInstrumentation()
        .AddHttpClientInstrumentation()
        .AddOtlpExporter(opt => opt.Endpoint = new Uri(jaegerUrl)));

builder.Services.AddHttpClient();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapGet("/order/{id}", async (int id, HttpClient client) => {
    var calc = await client.GetFromJsonAsync<object>($"{calcServiceUrl}/calculate/{id}");
    return Results.Ok(new { OrderId = id, Calculation = calc, Source = "K8s-Cluster" });
});

app.Run("http://0.0.0.0:5001");