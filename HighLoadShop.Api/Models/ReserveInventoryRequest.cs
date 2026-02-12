using HighLoadShop.Application.InventoryContext.Commands.ReserveInventory;

namespace HighLoadShop.Api.Models;

public record ReserveInventoryRequest(Guid OrderId, List<ReservationRequest> Items);
