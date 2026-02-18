using InventoryService.Application.Common.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryService.Application.Common;

public class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public async Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> request, CancellationToken cancellationToken)
    {
        return await SendAsync(request as IRequest<TResponse>, cancellationToken).ConfigureAwait(false);
    }

    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> request, CancellationToken cancellationToken)
    {
        return await SendAsync(request as IRequest<TResponse>, cancellationToken).ConfigureAwait(false);
    }

    private async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
    {
        var handlerType = typeof(IRequestHandler<,>)
             .MakeGenericType(request.GetType(), typeof(TResponse));

        dynamic handler = serviceProvider.GetRequiredService(handlerType);

        return await handler.HandleAsync((dynamic)request, cancellationToken);
    }
}
