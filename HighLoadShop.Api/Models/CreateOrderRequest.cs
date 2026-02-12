using HighLoadShop.Application.OrderContext.Commands.CreateOrder;

namespace HighLoadShop.Api.Models;

public record CreateOrderRequest(Guid UserId, List<OrderItemRequest> Items);
