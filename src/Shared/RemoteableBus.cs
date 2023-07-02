using MediatR;

namespace Shared;

// This is the bus for the UI
public class RemoteableBus : Bus
{
    private readonly GatewayPublisher _gateway;

    public RemoteableBus(IMediator mediator, GatewayPublisher gateway) : base(mediator)
    {
        _gateway = gateway;
    }

    public override Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
    {
        // not handling calls that don't return data right now
        throw new NotImplementedException();
    }

    public override async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is IRemoteableRequest remoteableRequest)
        {
            // Call the Gateway
            var response = await _gateway.Publish(remoteableRequest);
            
            // Extract body from response
            var result = Deserializer.Deserialize<TResponse>(response);
            
            return result;
        }

        // use MediatR locally
        return await base.Send(request, cancellationToken);
    }
}
