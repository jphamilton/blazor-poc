
using Grpc.Gateway.Service;
using MediatR;
using Newtonsoft.Json;
using System.Text;

namespace Shared;

// Yes, this is crap. I will add a better serializer later once the concept is proven.
public static class Serializer
{
    public static GatewayEnvelope Serialize(object obj)
    {
        var message = new GatewayEnvelope
        {
            Type = obj.GetType().ToString(),
        };

        var json = JsonConvert.SerializeObject(obj);
        
        message.Body = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
        
        return message;
    }
}

public static class Deserializer
{
    public static T Deserialize<T>(GatewayEnvelope gatewayEnvelope)
    {
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(gatewayEnvelope.Body));
        var result = JsonConvert.DeserializeObject<T>(json);
        return result;
    }

    public static object Deserialize(GatewayEnvelope gatewayEnvelope)
    {
        var type = Type.GetType(gatewayEnvelope.Type);
        var json = Encoding.UTF8.GetString(Convert.FromBase64String(gatewayEnvelope.Body));
        var result = JsonConvert.DeserializeObject(json, type);
        return result;
    }
}