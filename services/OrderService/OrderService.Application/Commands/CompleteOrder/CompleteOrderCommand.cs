using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Commands.CompleteOrder;

public record CompleteOrderCommand(Guid OrderId) : ICommand<Result>;
