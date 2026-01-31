namespace HighLoadShop.Application.Common.Interfaces;

public interface IDispatcher
{
    Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken);

    Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken);
}
