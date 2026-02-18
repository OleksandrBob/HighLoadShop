using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.Common.Models;
using InventoryService.Application.Interfaces;

namespace InventoryService.Application.Queries.GetInventory;

public class GetInventoryQueryHandler : IQueryHandler<GetInventoryQuery, Result<InventoryDto>>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetInventoryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<Result<InventoryDto>> HandleAsync(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var inventoryItem = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);
            if (inventoryItem == null)
                return Result.Failure<InventoryDto>("Product not found in inventory");

            var inventoryDto = new InventoryDto(
                inventoryItem.ProductId,
                inventoryItem.TotalQuantity,
                inventoryItem.ReservedQuantity,
                inventoryItem.AvailableQuantity);

            return Result.Success(inventoryDto);
        }
        catch (Exception ex)
        {
            return Result.Failure<InventoryDto>(ex.Message);
        }
    }
}
