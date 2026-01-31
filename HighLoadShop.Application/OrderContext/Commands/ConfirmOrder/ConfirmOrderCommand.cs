using HighLoadShop.Application.Common.Interfaces;
using HighLoadShop.Application.Common.Models;

namespace HighLoadShop.Application.OrderContext.Commands.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId) : ICommand<Result>;
