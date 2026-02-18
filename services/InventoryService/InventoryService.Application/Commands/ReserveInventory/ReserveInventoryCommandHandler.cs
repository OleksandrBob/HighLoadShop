using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.Common.Models;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Commands.ReserveInventory;

public class ReserveInventoryCommandHandler : ICommandHandler<ReserveInventoryCommand, Result<bool>>
{
    private readonly IInventoryRepository _inventoryRepository;

    public ReserveInventoryCommandHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<Result<bool>> HandleAsync(ReserveInventoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Check if all products have sufficient inventory
            foreach (var item in request.Items)
            {
                var inventoryItem = await _inventoryRepository.GetByProductIdAsync(item.ProductId, cancellationToken);
                if (inventoryItem == null)
                    return Result.Failure<bool>($"Product {item.ProductId} not found in inventory");

                if (!inventoryItem.CanReserve(item.Quantity))
                    return Result.Failure<bool>($"Insufficient inventory for product {item.ProductId}. Available: {inventoryItem.AvailableQuantity}, Requested: {item.Quantity}");
            }

            // Reserve all items
            var expiresAt = DateTime.UtcNow.AddMinutes(15);
            foreach (var item in request.Items)
            {
                var inventoryItem = await _inventoryRepository.GetByProductIdAsync(item.ProductId, cancellationToken);
                inventoryItem!.Reserve(item.Quantity);

                var reservation = new Reservation(request.OrderId, item.ProductId, item.Quantity, expiresAt);
                _inventoryRepository.AddReservation(reservation);

                _inventoryRepository.Update(inventoryItem);
            }

            await _inventoryRepository.SaveChangesAsync(cancellationToken);

            return Result.Success(true);
        }
        catch (Exception ex)
        {
            return Result.Failure<bool>(ex.Message);
        }
    }
}
