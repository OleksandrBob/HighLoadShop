using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid UserId,
    List<OrderItemRequest> Items) : ICommand<Result<Guid>>;

public record OrderItemRequest(
    Guid ProductId,
    int Quantity);
