using OrderService.Domain.Common;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Entities.Events;

public record OrderConfirmedDomainEvent(
    Guid OrderId,
    Guid UserId,
    IReadOnlyList<OrderItem> OrderItems) : DomainEvent;

public record OrderCancelledDomainEvent(
    Guid OrderId,
    IReadOnlyList<OrderItem> OrderItems) : DomainEvent;

public record OrderCompletedDomainEvent(
    Guid OrderId,
    Guid UserId) : DomainEvent;
