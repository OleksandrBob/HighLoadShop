using InventoryService.Application.Commands.ReserveInventory;

namespace InventoryService.Api.Models;

public record ReserveInventoryRequest(Guid OrderId, List<ReservationItemRequest> Items);
