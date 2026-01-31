using HighLoadShop.Application.InventoryContext.Interfaces;
using HighLoadShop.Domain.InventoryContext.Entities;
using Microsoft.EntityFrameworkCore;

namespace HighLoadShop.Persistence.InventoryContext.Repositories;

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
            .FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken);
    }

    public async Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default)
    {
        await _context.InventoryItems.AddAsync(inventoryItem, cancellationToken);
    }

    public Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default)
    {
        _context.InventoryItems.Update(inventoryItem);
        return Task.CompletedTask;
    }

    public async Task<Reservation?> GetReservationByIdAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId, cancellationToken);
    }

    public async Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await _context.Reservations.AddAsync(reservation, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}