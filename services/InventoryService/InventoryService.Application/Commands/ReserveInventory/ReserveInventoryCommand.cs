using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.Common.Models;

namespace InventoryService.Application.Commands.ReserveInventory;

public record ReserveInventoryCommand(
    Guid OrderId,
    List<ReservationItemRequest> Items) : ICommand<Result<bool>>;

public record ReservationItemRequest(
    Guid ProductId,
    int Quantity);
