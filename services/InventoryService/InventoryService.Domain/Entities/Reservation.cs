using InventoryService.Domain.Common;

namespace InventoryService.Domain.Entities;

public class Reservation : Entity<Guid>
{
    public Guid OrderId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    private Reservation() { }

    public Reservation(Guid orderId, Guid productId, int quantity, DateTime expiresAt) : base(Guid.NewGuid())
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive");

        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        ExpiresAt = expiresAt;
    }

    public void ExtendExpiration(DateTime newExpirationTime)
    {
        if (newExpirationTime <= ExpiresAt)
            throw new ArgumentException("New expiration time must be later than current expiration");

        ExpiresAt = newExpirationTime;
    }
}
