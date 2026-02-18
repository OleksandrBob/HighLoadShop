using InventoryService.Domain.Entities;

namespace InventoryService.Application.Interfaces;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<List<InventoryItem>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(InventoryItem inventoryItem);
    void Update(InventoryItem inventoryItem);
    void AddReservation(Reservation reservation);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
