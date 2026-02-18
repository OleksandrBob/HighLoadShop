using InventoryService.Domain.Common;

namespace InventoryService.Domain.Entities.Events;

public record InventoryReservedDomainEvent(
    Guid ProductId,
    int Quantity) : DomainEvent;

public record InventoryReservationReleasedDomainEvent(
    Guid ProductId,
    int Quantity) : DomainEvent;

public record InventoryConfirmedDomainEvent(
    Guid ProductId,
    int Quantity) : DomainEvent;

public record StockAddedDomainEvent(
    Guid ProductId,
    int Quantity) : DomainEvent;
