using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Queries.GetOrder;

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
