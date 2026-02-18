using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Persistence.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _context;

    public InventoryRepository(InventoryDbContext context)
    {
        _context = context;
    }

    public async Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .FirstOrDefaultAsync(i => i.ProductId == productId, cancellationToken);
    }

    public async Task<List<InventoryItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.InventoryItems
            .OrderBy(i => i.ProductId)
            .ToListAsync(cancellationToken);
    }

    public void Add(InventoryItem inventoryItem)
    {
        _context.InventoryItems.Add(inventoryItem);
    }

    public void Update(InventoryItem inventoryItem)
    {
        _context.InventoryItems.Update(inventoryItem);
    }

    public void AddReservation(Reservation reservation)
    {
        _context.Reservations.Add(reservation);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
