using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;

namespace HighLoadShop.Application.InventoryContext.Queries.GetInventory;

public record GetInventoryQuery(Guid ProductId) : IQuery<Result<InventoryDto>>;

public record InventoryDto(
    Guid ProductId,
    int TotalQuantity,
    int ReservedQuantity,
    int AvailableQuantity);