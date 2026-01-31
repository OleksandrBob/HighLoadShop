using HighLoadShop.Domain.InventoryContext.Entities;

namespace HighLoadShop.Application.InventoryContext.Interfaces;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
    Task<Reservation?> GetReservationByIdAsync(Guid reservationId, CancellationToken cancellationToken = default);
    Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}