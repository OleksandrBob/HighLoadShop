using HighLoadShop.Domain.Common;
using HighLoadShop.Domain.PaymentContext.Events;

namespace HighLoadShop.Domain.PaymentContext.Entities;

public class Payment : AggregateRoot<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = string.Empty;
    public PaymentStatus Status { get; private set; }
    public PaymentMethod Method { get; private set; }
    public string? TransactionId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ProcessedAt { get; private set; }

    private Payment() { }

    public Payment(Guid orderId, Guid userId, decimal amount, string currency, PaymentMethod method) : base(Guid.NewGuid())
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty");

        OrderId = orderId;
        UserId = userId;
        Amount = amount;
        Currency = currency;
        Method = method;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentInitiatedDomainEvent(Id, OrderId, Amount, Currency));
    }

    public void Process(string transactionId)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be processed");

        TransactionId = transactionId;
        Status = PaymentStatus.Completed;
        ProcessedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentCompletedDomainEvent(Id, OrderId, Amount, TransactionId));
    }

    public void Fail(string reason)
    {
        if (Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Only pending payments can be marked as failed");

        Status = PaymentStatus.Failed;
        ProcessedAt = DateTime.UtcNow;

        RaiseDomainEvent(new PaymentFailedDomainEvent(Id, OrderId, reason));
    }
}

public enum PaymentStatus
{
    Pending = 0,
    Completed = 1,
    Failed = 2,
    Refunded = 3
}

public enum PaymentMethod
{
    CreditCard = 0,
    PayPal = 1,
    BankTransfer = 2,
    Cryptocurrency = 3
}