namespace Shared;

// This is the bus for the UI
public class RemoteableBus : IBus
{
    private readonly GatewayPublisher _gateway;

    public RemoteableBus(GatewayPublisher gateway)
    {
        _gateway = gateway;
    }

    public async Task<TResponse> Send<TResponse>(IMessage<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is IRemoteableRequest<TResponse> remoteableRequest)
        {
            // Call the Gateway
            var response = await _gateway.Publish(remoteableRequest);

            // Extract body from response
            var result = Deserializer.Deserialize<TResponse>(response);

            return result;
        }

        throw new Exception("Request is not remoteable");
    }
}
