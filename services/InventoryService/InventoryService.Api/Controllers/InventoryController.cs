using InventoryService.Api.Filters;
using InventoryService.Api.Models;
using InventoryService.Application.Common.Interfaces;
using InventoryService.Application.Commands.ReserveInventory;
using InventoryService.Application.Queries.GetInventory;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class InventoryController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public InventoryController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Reserves inventory for an order
    /// </summary>
    [HttpPost("reserve")]
    [TypeFilter(typeof(ValidationActionFilter<ReserveInventoryRequest>))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReserveInventory(
        [FromBody] ReserveInventoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ReserveInventoryCommand(request.OrderId, request.Items);
        var result = await _dispatcher.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Ok(new { Success = result.Value })
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Gets inventory information for a product
    /// </summary>
    [HttpGet("{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetInventory(
        Guid productId,
        CancellationToken cancellationToken)
    {
        var query = new GetInventoryQuery(productId);
        var result = await _dispatcher.SendAsync(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }
}
