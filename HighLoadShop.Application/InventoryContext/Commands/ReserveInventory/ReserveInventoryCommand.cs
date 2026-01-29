using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;

namespace HighLoadShop.Application.InventoryContext.Commands.ReserveInventory;

public record ReserveInventoryCommand(
    Guid OrderId,
    List<ReservationRequest> Items) : ICommand<Result>;

public record ReservationRequest(
    Guid ProductId,
    int Quantity);