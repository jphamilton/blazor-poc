namespace Shared;

public interface IBus
{
    Task<TResponse> Send<TResponse>(IMessage<TResponse> request, CancellationToken cancellationToken = default);
}
