namespace InventoryService.Domain.Common;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}

public abstract record DomainEvent(DateTime OccurredOn) : IDomainEvent
{
    protected DomainEvent() : this(DateTime.UtcNow) { }
}
