using HighLoadShop.Domain.Common;

namespace HighLoadShop.Domain.PaymentContext.Events;

public record PaymentInitiatedDomainEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    string Currency) : DomainEvent;

public record PaymentCompletedDomainEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    string TransactionId) : DomainEvent;

public record PaymentFailedDomainEvent(
    Guid PaymentId,
    Guid OrderId,
    string Reason) : DomainEvent;