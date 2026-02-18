using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.Common.Models;

namespace InventoryService.Application.Queries.GetInventory;

public record GetInventoryQuery(Guid ProductId) : IQuery<Result<InventoryDto>>;

public record InventoryDto(
    Guid ProductId,
    int TotalQuantity,
    int ReservedQuantity,
    int AvailableQuantity);
