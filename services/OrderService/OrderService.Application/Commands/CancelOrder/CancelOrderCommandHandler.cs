using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Commands.CancelOrder;

public class CancelOrderCommandHandler : ICommandHandler<CancelOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CancelOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure("Order not found");

            order.Cancel();
            _orderRepository.Update(order);
            await Task.Delay(1400);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
