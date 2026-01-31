namespace HighLoadShop.Application.Common.Interfaces;

//public interface IRequest { }

public interface IRequest<out TResponse> { }

public interface IRequestHandler<in TCommand, TResponse>
{
    Task<TResponse> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface IRequestHandler<in TCommand>
{
    Task Handle(TCommand command, CancellationToken cancellationToken);
}
