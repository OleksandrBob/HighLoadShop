using OrderService.Domain.Common;
using OrderService.Domain.Entities.Events;

namespace OrderService.Domain.Entities;

public class Order : AggregateRoot<Guid>
{
    private readonly List<OrderItem> _orderItems = new();

    public Guid UserId { get; private set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public OrderStatus Status { get; private set; }
    public DateTime ReservedUntil { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Order() { }

    public Order(Guid id, Guid userId) : base(id)
    {
        UserId = userId;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        ReservedUntil = DateTime.UtcNow.AddMinutes(15);
    }

    public void AddOrderItem(Guid productId, int quantity)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify order items for non-pending orders");

        var existingItem = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (existingItem != null)
        {
            existingItem.UpdateQuantity(existingItem.Quantity + quantity);
        }
        else
        {
            _orderItems.Add(new OrderItem(productId, quantity));
        }
    }

    public void RemoveOrderItem(Guid productId)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot modify order items for non-pending orders");

        var item = _orderItems.FirstOrDefault(x => x.ProductId == productId);
        if (item != null)
        {
            _orderItems.Remove(item);
        }
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");

        Status = OrderStatus.Confirmed;
        RaiseDomainEvent(new OrderConfirmedDomainEvent(Id, UserId, _orderItems.ToList()));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed || Status == OrderStatus.Cancelled)
            throw new InvalidOperationException("Cannot cancel completed or already cancelled orders");

        Status = OrderStatus.Cancelled;
        RaiseDomainEvent(new OrderCancelledDomainEvent(Id, _orderItems.ToList()));
    }

    public void Complete()
    {
        if (Status != OrderStatus.Confirmed)
            throw new InvalidOperationException("Only confirmed orders can be completed");

        Status = OrderStatus.Completed;
        RaiseDomainEvent(new OrderCompletedDomainEvent(Id, UserId));
    }

    public bool IsExpired => DateTime.UtcNow > ReservedUntil && Status == OrderStatus.Pending;

    public void ExtendReservation(int minutesToAdd)
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can have reservation extended");

        ReservedUntil = ReservedUntil.AddMinutes(minutesToAdd);
    }
}
