using MediatR;

namespace Shared;

public interface IBus
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
        where TRequest : IRequest;
}

public interface IRemoteableRequest
{
    // Marker interface
}