using MediatR;

namespace Shared;

public interface IMessage<TResponse> : IRequest<TResponse>
{

}