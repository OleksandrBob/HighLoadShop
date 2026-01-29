using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;

namespace HighLoadShop.Application.OrderContext.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid UserId,
    List<OrderItemRequest> Items) : ICommand<Result<Guid>>;

public record OrderItemRequest(
    Guid ProductId,
    int Quantity);