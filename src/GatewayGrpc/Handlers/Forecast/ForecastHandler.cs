using Google.Protobuf.WellKnownTypes;
using Grpc.WeatherForecast.Service;
using MediatR;
using Shared.Models;
using Shared.Queries;

namespace GatewayGrpc.Handlers.Forecast;

public class ForecastHandler : IRequestHandler<ForecastQuery, IEnumerable<WeatherForecast>>
{
    private readonly ForecastService.ForecastServiceClient _client;

    public ForecastHandler(ForecastService.ForecastServiceClient client)
    {
        _client = client;
    }

    public Task<IEnumerable<WeatherForecast>> Handle(ForecastQuery request, CancellationToken cancellationToken)
    {
        var forecasts = _client.GetForecasts(new Empty());
        
        // transform to view model
        return Task.FromResult(forecasts.Forecasts.Select(f => new WeatherForecast
        {
            Date = f.Date.ToDateTime(),
            Summary = f.Summary,
            TemperatureC = f.TemperatureC
        }));
    }
}
