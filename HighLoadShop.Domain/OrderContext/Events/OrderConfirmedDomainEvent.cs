using HighLoadShop.Domain.Common;
using HighLoadShop.Domain.OrderContext.Entities;

namespace HighLoadShop.Domain.OrderContext.Events;

public record OrderConfirmedDomainEvent(
    Guid OrderId,
    Guid UserId,
    IReadOnlyList<OrderItem> OrderItems) : DomainEvent;