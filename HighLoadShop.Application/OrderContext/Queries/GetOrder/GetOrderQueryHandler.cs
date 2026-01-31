using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;
using HighLoadShop.Application.OrderContext.Interfaces;

namespace HighLoadShop.Application.OrderContext.Queries.GetOrder;

public class GetOrderQueryHandler : IQueryHandler<GetOrderQuery, Result<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderDto>> HandleAsync(GetOrderQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
                return Result.Failure<OrderDto>("Order not found");

            var orderDto = new OrderDto(
                order.Id,
                order.UserId,
                order.OrderItems.Select(x => new OrderItemDto(x.ProductId, x.Quantity)).ToList(),
                order.Status.ToString(),
                order.ReservedUntil,
                order.CreatedAt);

            return Result.Success(orderDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<OrderDto>(ex.Message);
        }
    }
}