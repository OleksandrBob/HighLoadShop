using HighLoadShop.Domain.Common;

namespace HighLoadShop.Domain.OrderContext.Events;

public record OrderCompletedDomainEvent(
    Guid OrderId,
    Guid UserId) : DomainEvent;