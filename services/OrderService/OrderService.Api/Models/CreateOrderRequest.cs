using OrderService.Application.Commands.CreateOrder;

namespace OrderService.Api.Models;

public record CreateOrderRequest(Guid UserId, List<OrderItemRequest> Items);
