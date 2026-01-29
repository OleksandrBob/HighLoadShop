using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;
using HighLoadShop.Application.InventoryContext.Commands.ReserveInventory;

namespace HighLoadShop.Application.InventoryContext.Queries.GetInventory;

public class GetInventoryQueryHandler : IQueryHandler<GetInventoryQuery, Result<InventoryDto>>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetInventoryQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<Result<InventoryDto>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
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