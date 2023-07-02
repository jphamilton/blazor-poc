using Shared.Models;

namespace UI.Features.Forecast.State;

public record ForecastGetAction();

public record ForecastSetAction(List<WeatherForecast> Forecasts);
