using OrderService.Application.Common.Interfaces;
using OrderService.Application.Common.Models;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands.CreateOrder;

public class CreateOrderCommandHandler : ICommandHandler<CreateOrderCommand, Result<Guid>>
{
    private readonly IOrderRepository _orderRepository;
    //private readonly IInventoryServiceClient _inventoryServiceClient;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository)
        //IInventoryServiceClient inventoryServiceClient)
    {
        _orderRepository = orderRepository;
        //_inventoryServiceClient = inventoryServiceClient;
    }

    public async Task<Result<Guid>> HandleAsync(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var orderId = Guid.NewGuid();
            var order = new Order(orderId, request.UserId);

            foreach (var item in request.Items)
            {
                order.AddOrderItem(item.ProductId, item.Quantity);
            }

            _orderRepository.Add(order);
            await _orderRepository.SaveChangesAsync(cancellationToken);

            // Reserve inventory via HTTP call to InventoryService
            //var reserveResult = await _inventoryServiceClient.ReserveInventoryAsync(
            //    orderId,
            //    request.Items.Select(i => new ReservationItem(i.ProductId, i.Quantity)).ToList(),
            //    cancellationToken);

            //if (!reserveResult)
            //{
            //    // If inventory reservation fails, cancel the order
            //    order.Cancel();
            //    await _orderRepository.SaveChangesAsync(cancellationToken);
            //    return Result.Failure<Guid>("Failed to reserve inventory");
            //}

            return Result.Success(orderId);
        }
        catch (Exception ex)
        {
            return Result.Failure<Guid>(ex.Message);
        }
    }
}
