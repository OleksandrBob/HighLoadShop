using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;

namespace OrderService.Application.Commands.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId) : ICommand<Result>;
