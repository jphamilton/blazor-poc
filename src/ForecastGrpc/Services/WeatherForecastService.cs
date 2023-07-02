using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.WeatherForecast.Service;

namespace Forecast.Services;

public class WeatherForecastService : ForecastService.ForecastServiceBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public override async Task<ForecastList> GetForecasts(Empty request, ServerCallContext context)
    {
        // simulate network latency
        await Task.Delay(800);

        return new ForecastList
        {
            Forecasts =
            {
                Enumerable.Range(1, 5).Select(index => new WeatherForecastMessage
                {
                    Date = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                })
            }
        };
    }
}
