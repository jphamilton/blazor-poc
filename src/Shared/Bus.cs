using MediatR;

namespace Shared;

// This bus is used in the gateway service
public class Bus : IBus
{
    private readonly IMediator _mediator;

    public Bus(IMediator mediator)
    {
        _mediator = mediator;
    }

    public virtual Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(request, cancellationToken);
    }

    public virtual Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        return _mediator.Send(request, cancellationToken);
    }
}
