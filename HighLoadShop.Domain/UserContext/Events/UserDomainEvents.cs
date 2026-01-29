using HighLoadShop.Domain.Common;

namespace HighLoadShop.Domain.UserContext.Events;

public record UserCreatedDomainEvent(
    Guid UserId,
    string Email) : DomainEvent;

public record UserDeactivatedDomainEvent(
    Guid UserId) : DomainEvent;

public record UserActivatedDomainEvent(
    Guid UserId) : DomainEvent;