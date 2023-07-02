using Grpc.Gateway.Service;

namespace Shared;

public class GatewayPublisher
{
    private readonly Gateway.GatewayClient _client;

    public GatewayPublisher(Gateway.GatewayClient client)
    {
        _client = client;
    }

    public async Task<GatewayEnvelope> Publish<TResponse>(IRemoteableRequest<TResponse> request)
    {
        GatewayEnvelope message = Serializer.Serialize(request);
        return await _client.PublishAsync(message);
    }
}
