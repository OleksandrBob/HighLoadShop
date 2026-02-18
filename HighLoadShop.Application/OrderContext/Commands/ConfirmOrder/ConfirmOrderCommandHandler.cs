using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;
using HighLoadShop.Application.OrderContext.Interfaces;

namespace HighLoadShop.Application.OrderContext.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler : ICommandHandler<ConfirmOrderCommand, Result>
{
    private readonly IOrderRepository _orderRepository;

    public ConfirmOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result> HandleAsync(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order is null)
                return Result.Failure("Order not found");

            order.Confirm();

            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}