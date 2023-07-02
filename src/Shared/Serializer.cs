
using Google.Protobuf;
using Grpc.Gateway.Service;
using Newtonsoft.Json;

namespace Shared;

public static class Serializer
{
    public static GatewayEnvelope Serialize(object obj)
    {
        return new GatewayEnvelope
        {
            Type = obj.GetType().ToString(),
            Body = ByteString.CopyFromUtf8(JsonConvert.SerializeObject(obj))
        };
    }
}

public static class Deserializer
{
    public static T? Deserialize<T>(GatewayEnvelope gatewayEnvelope)
    {
        return JsonConvert.DeserializeObject<T>(gatewayEnvelope.Body.ToStringUtf8());
    }

    public static object? Deserialize(GatewayEnvelope gatewayEnvelope)
    {
        return JsonConvert.DeserializeObject(gatewayEnvelope.Body.ToStringUtf8(), Type.GetType(gatewayEnvelope.Type)!);
    }
}