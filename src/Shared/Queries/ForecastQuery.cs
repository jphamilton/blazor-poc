using Shared.Models;

namespace Shared.Queries;

public record ForecastQuery(DateTime StartDate) : IMessage<IEnumerable<WeatherForecast>>, IRemoteableRequest;
