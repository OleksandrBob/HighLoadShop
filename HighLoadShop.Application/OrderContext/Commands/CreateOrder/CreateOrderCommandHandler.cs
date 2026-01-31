using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;
using HighLoadShop.Application.OrderContext.Interfaces;
using HighLoadShop.Domain.OrderContext.Entities;

namespace HighLoadShop.Application.OrderContext.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<Guid>> HandleAsync(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var orderId = Guid.NewGuid();
            var order = new Order(orderId, request.UserId);

            foreach (var item in request.Items)
            {
                order.AddOrderItem(item.ProductId, item.Quantity);
            }

            await _orderRepository.AddAsync(order, cancellationToken);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(orderId);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
    }
}
