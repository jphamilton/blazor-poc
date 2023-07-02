using GatewayGrpc.Services;
using Grpc.WeatherForecast.Service;
using Shared;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddTransient<IBus, Bus>();

builder.Services.AddGrpcClient<ForecastService.ForecastServiceClient>(o =>
{
    o.Address = new Uri(builder.Configuration.GetValue<string>("GrpcEndpoints:ForecastService"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GatewayService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
