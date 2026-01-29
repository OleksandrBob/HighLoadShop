using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;

namespace HighLoadShop.Application.OrderContext.Queries.GetOrder;

public record GetOrderQuery(Guid OrderId) : IQuery<Result<OrderDto>>;

public record OrderDto(
    Guid OrderId,
    Guid UserId,
    List<OrderItemDto> Items,
    string Status,
    DateTime ReservedUntil,
    DateTime CreatedAt);

public record OrderItemDto(
    Guid ProductId,
    int Quantity);