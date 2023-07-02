# Blazor POC

This POC explores state management with Fluxor (Redux) and handling all service calls through a single gRPC endpoint, essentially MediatR over gRPC.

To run, configure solution to start multiple projects: Forecasts, Gateway, and UI.

![Data Flow](https://github.com/jphamilton/blazor-poc/blob/master/img/flow.png?raw=true "Data Flow")

## Fluxor
The Fluxor portion is adapted from the [Advanced Blazor State Management Using Fluxor](https://dev.to/mr_eking/advanced-blazor-state-management-using-fluxor-part-1-696) by Eric King.
This POC shows how Fluxor can be used to not only manage state, but also create complex interactions between components.

## Bus Pattern for gRPC service calls
This was adapted from a talk by Jeffrey Palermo on [Blazor Architecure Patterns](https://www.youtube.com/watch?v=SxfUHLAfC8k).

### Gateway Service ###
This is the single gRPC endpoint for the entire UI.

```
service Gateway {
  rpc Publish (GatewayEnvelope) returns (GatewayEnvelope);
}

message GatewayEnvelope {
  string Type = 1;  // C# type name of the request
  string Body = 2;  // Serialized MediatR request
}
```

<code>Body</code> is basically a serialized MediatR request. The Gateway deserializes the request and uses MediatR to send it to the appropriate handler. The handler is responsible for
orchestrating service calls, mapping protobuf back to Models, etc. The response from the handler is serialized and returned back to the caller as a <code>GatewayEnvelope</code>.

The Gateway knows about all of the available services, but the UI only knows about the Gateway.

### Bus ###
Instead of calling the gRPC service directly, a Bus is used.

```
// ForecastQuery.cs
public record ForecastQuery(DateTime StartDate) : IRemoteableRequest<IEnumerable<WeatherForecast>>;

// elsewhere
var forecasts = await _bus.Send(new ForecastQuery(DateTime.UtcNow));
```

The Bus on the UI side calls the GatwayPublisher, which is a simple wrapper around the gRPC client. This could be extended
to include identity, claims, roles, etc.

```
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

```

