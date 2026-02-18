using InventoryService.Domain.Common;
using InventoryService.Domain.Entities.Events;

namespace InventoryService.Domain.Entities;

public class InventoryItem : AggregateRoot<Guid>
{
    public Guid ProductId { get; private set; }
    public int TotalQuantity { get; private set; }
    public int ReservedQuantity { get; private set; }

    public int AvailableQuantity => TotalQuantity - ReservedQuantity;

    private InventoryItem() { }

    public InventoryItem(Guid productId, int totalQuantity) : base(Guid.NewGuid())
    {
        if (totalQuantity < 0)
            throw new ArgumentException("Total quantity cannot be negative");

        ProductId = productId;
        TotalQuantity = totalQuantity;
        ReservedQuantity = 0;
    }

    public bool CanReserve(int quantity)
    {
        return AvailableQuantity >= quantity;
    }

    public void Reserve(int quantity)
    {
        if (!CanReserve(quantity))
            throw new InvalidOperationException($"Cannot reserve {quantity} items. Available: {AvailableQuantity}");

        ReservedQuantity += quantity;
        RaiseDomainEvent(new InventoryReservedDomainEvent(ProductId, quantity));
    }

    public void ReleaseReservation(int quantity)
    {
        if (quantity > ReservedQuantity)
            throw new InvalidOperationException($"Cannot release {quantity} items. Reserved: {ReservedQuantity}");

        ReservedQuantity -= quantity;
        RaiseDomainEvent(new InventoryReservationReleasedDomainEvent(ProductId, quantity));
    }

    public void ConfirmReservation(int quantity)
    {
        if (quantity > ReservedQuantity)
            throw new InvalidOperationException($"Cannot confirm {quantity} items. Reserved: {ReservedQuantity}");

        ReservedQuantity -= quantity;
        TotalQuantity -= quantity;
        RaiseDomainEvent(new InventoryConfirmedDomainEvent(ProductId, quantity));
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive");

        TotalQuantity += quantity;
        RaiseDomainEvent(new StockAddedDomainEvent(ProductId, quantity));
    }
}
