using HighLoadShop.Api.Models;
using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.InventoryContext.Commands.ReserveInventory;
using HighLoadShop.Application.InventoryContext.Queries.GetInventory;
using Microsoft.AspNetCore.Mvc;

namespace HighLoadShop.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class InventoryController(
    IDispatcher dispatcher)
    : ControllerBase
{
    /// <summary>
    /// Gets inventory for a product
    /// </summary>
    [HttpGet("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInventory(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var query = new GetInventoryQuery(productId);
        var result = await dispatcher.SendAsync(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    /// <summary>
    /// Reserves inventory for an order
    /// </summary>
    [HttpPost("reserve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReserveInventory(
        [FromBody] ReserveInventoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveInventoryCommand(request.OrderId, request.Items);
        var result = await dispatcher.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error);
    }
}
