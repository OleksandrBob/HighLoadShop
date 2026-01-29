using HighLoadShop.Domain.Common;
using HighLoadShop.Domain.OrderContext.Entities;

namespace HighLoadShop.Domain.OrderContext.Events;

public record OrderCancelledDomainEvent(
    Guid OrderId,
    IReadOnlyList<OrderItem> OrderItems) : DomainEvent;