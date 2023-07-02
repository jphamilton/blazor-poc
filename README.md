# Blazor POC

This POC explores state management with Fluxor and handling all service calls through a single gRPC endpoint.

To run, configure solution to start multiple projects: Forecasts, Gateway, and UI.

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

<code>Body</code> is a serialized MediatR request (See ForecastQuery.cs). 
The Gateway deserializes the request and uses MediatR to send it to the appropriate handler. The handler is responsible for
orchestrating service calls, mapping protobuf back to Models, etc. The response is serialized and returned back as a <code>GatewayEnvelope</code>.

The Gateway knows about all of the available services, but the UI only knows about the Gateway.

### Bus ###
Instead of calling the gRPC service directly, an Action is dispatched using Fluxor.

```
@* Forecast.razor *@
private void LoadForecasts()
{
    Dispatcher.Dispatch(new ForecastGetAction());
}
```

In the POC, ForecastGetAction is "intercepted" in ForecastEffect, which in turn uses the Bus to send a ForecastQuery.

```
// ForecastQuery.cs
public record ForecastQuery(DateTime StartDate) : IRequest<IEnumerable<WeatherForecast>>, IRemoteableRequest; // MediatR request

// ForecastEffects.cs
[EffectMethod]
public async Task LoadForecasts(ForecastGetAction _, IDispatcher dispatcher)
{
    var forecasts = await _bus.Send(new ForecastQuery(DateTime.UtcNow));

    // update our app state with the results (see ForecastReducers.cs)
    dispatcher.Dispatch(new ForecastSetAction(forecasts.ToList()));
}
```

The Bus on the UI side simply calls the GatwayPublisher, which is a simple wrapper around the gRPC client.

```
public class RemoteableBus : Bus
{
    private readonly GatewayPublisher _gateway;

    public RemoteableBus(IMediator mediator, GatewayPublisher gateway) : base(mediator)
    {
        _gateway = gateway;
    }

    public override async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        if (request is IRemoteableRequest remoteableRequest)
        {
            // Call the Gateway
            var response = await _gateway.Publish(remoteableRequest);
            
            // Extract body from response
            var result = Deserializer.Deserialize<TResponse>(response);
            
            return result; // to UI
        }

        // use MediatR locally
        return await base.Send(request, cancellationToken);
    }
}
```
