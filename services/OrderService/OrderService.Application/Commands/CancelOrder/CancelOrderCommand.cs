using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : ICommand<Result>;
