using MediatR;
using Shared.Models;

namespace Shared.Queries;

public record ForecastQuery(DateTime StartDate) : IRequest<IEnumerable<WeatherForecast>>, IRemoteableRequest;
