using OrderService.Api.Filters;
using OrderService.Api.Models;
using OrderService.Application.Common.Interfaces;
using OrderService.Application.Commands.ConfirmOrder;
using OrderService.Application.Commands.CreateOrder;
using OrderService.Application.Queries.GetOrder;
using Microsoft.AspNetCore.Mvc;

namespace OrderService.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IDispatcher _dispatcher;

    public OrdersController(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    /// <summary>
    /// Creates a new order
    /// </summary>
    [HttpPost]
    [TypeFilter(typeof(ValidationActionFilter<CreateOrderRequest>))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrder(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateOrderCommand(request.UserId, request.Items);
        var result = await _dispatcher.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetOrder), new { orderId = result.Value }, new { OrderId = result.Value })
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Gets an order by ID
    /// </summary>
    [HttpGet("{orderId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var query = new GetOrderQuery(orderId);
        var result = await _dispatcher.SendAsync(query, cancellationToken);

        return result.IsSuccess
            ? Ok(result.Value)
            : NotFound(result.Error);
    }

    /// <summary>
    /// Confirms an order
    /// </summary>
    [HttpPost("{orderId:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ConfirmOrder(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var command = new ConfirmOrderCommand(orderId);
        var result = await _dispatcher.SendAsync(command, cancellationToken);

        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error);
    }
}
