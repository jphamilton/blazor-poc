﻿using Grpc.Gateway.Service;
using Newtonsoft.Json;
using System.Text;

namespace Shared;

public class GatewayPublisher
{
    private readonly Gateway.GatewayClient _client;

    public GatewayPublisher(Gateway.GatewayClient client)
    {
        _client = client;
    }

    public async Task<GatewayEnvelope> Publish(IRemoteableRequest request)
    {
        GatewayEnvelope message = Serializer.Serialize(request);
        var result = await _client.PublishAsync(message);
        return result;
    }

    
}