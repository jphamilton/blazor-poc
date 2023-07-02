using Shared.Models;

namespace Shared.Queries;

public record ForecastQuery(DateTime StartDate) : IRemoteableRequest<IEnumerable<WeatherForecast>>;
