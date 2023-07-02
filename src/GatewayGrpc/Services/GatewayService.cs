using Grpc.Core;
using Grpc.Gateway.Service;
using MediatR;
using Shared;

namespace GatewayGrpc.Services;

// Only a single endpoint to secure. This handles every request from the UI.

public class GatewayService : Gateway.GatewayBase
{
    private readonly IMediator _mediator;

    public GatewayService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task<GatewayEnvelope> Publish(GatewayEnvelope request, ServerCallContext context)
    {
        // Envelope body is a MediatR request
        var message = Deserializer.Deserialize(request);

        // Send message to Handler
        var result = await _mediator.Send(message);

        // Put response back into a GatewayEnvelope and return
        var response = Serializer.Serialize(result);
        
        return response;
    }
}
