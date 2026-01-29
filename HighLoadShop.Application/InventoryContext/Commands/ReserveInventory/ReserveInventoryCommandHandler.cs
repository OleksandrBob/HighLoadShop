using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;
using HighLoadShop.Domain.InventoryContext.Entities;

namespace HighLoadShop.Application.InventoryContext.Commands.ReserveInventory;

public class ReserveInventoryCommandHandler : ICommandHandler<ReserveInventoryCommand, Result>
{
    private readonly IInventoryRepository _inventoryRepository;

    public ReserveInventoryCommandHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<Result> Handle(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var reservations = new List<Reservation>();

            foreach (var item in request.Items)
            {
                var inventoryItem = await _inventoryRepository.GetByProductIdAsync(item.ProductId, cancellationToken);
                if (inventoryItem == null)
                    return Result.Failure($"Product {item.ProductId} not found in inventory");

                if (!inventoryItem.CanReserve(item.Quantity))
                    return Result.Failure($"Insufficient inventory for product {item.ProductId}");

                inventoryItem.Reserve(item.Quantity);
                
                var reservation = new Reservation(
                    request.OrderId,
                    item.ProductId,
                    item.Quantity,
                    DateTime.UtcNow.AddMinutes(15)); // 15-minute reservation
                
                reservations.Add(reservation);
                
                await _inventoryRepository.UpdateAsync(inventoryItem, cancellationToken);
            }

            foreach (var reservation in reservations)
            {
                await _inventoryRepository.AddReservationAsync(reservation, cancellationToken);
            }

            await _inventoryRepository.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
    Task UpdateAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
    Task<Reservation?> GetReservationByIdAsync(Guid reservationId, CancellationToken cancellationToken = default);
    Task AddReservationAsync(Reservation reservation, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}