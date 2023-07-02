using Fluxor;
using Shared.Models;

namespace UI.Features.Forecast.State;

public record ForecastState
{
    public required List<WeatherForecast> Forecasts { get; init; }
    public bool Initialized { get; init; }
    public bool IsLoading { get; init; }
}

public class ForecastFeature : Feature<ForecastState>
{
    public override string GetName() => "Forecasts";

    protected override ForecastState GetInitialState()
    {
        return new ForecastState
        {
            Forecasts = new(),
            IsLoading = false,
            Initialized = false
        };
    }
}
