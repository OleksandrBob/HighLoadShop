
using HighLoadShop.Application;
using HighLoadShop.Infrastructure;
using HighLoadShop.Persistence;
using HighLoadShop.Application.OrderContext.Commands.CreateOrder;
using HighLoadShop.Application.OrderContext.Commands.ConfirmOrder;
using HighLoadShop.Application.OrderContext.Queries.GetOrder;
using HighLoadShop.Application.InventoryContext.Commands.ReserveInventory;
using HighLoadShop.Application.InventoryContext.Queries.GetInventory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HighLoadShopApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Add Clean Architecture layers
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddPersistence(builder.Configuration);

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // API v1 Routes
            var v1 = app.MapGroup("/api/v1");

            // Order Context Endpoints
            var orders = v1.MapGroup("/orders").WithTags("Orders");

            orders.MapPost("/", async ([FromBody] CreateOrderRequest request, IMediator mediator) =>
            {
                var command = new CreateOrderCommand(request.UserId, request.Items);
                var result = await mediator.Send(command);

                return result.IsSuccess 
                    ? Results.Created($"/api/v1/orders/{result.Value}", new { OrderId = result.Value })
                    : Results.BadRequest(result.Error);
            });

            orders.MapGet("/{orderId:guid}", async (Guid orderId, IMediator mediator) =>
            {
                var query = new GetOrderQuery(orderId);
                var result = await mediator.Send(query);

                return result.IsSuccess 
                    ? Results.Ok(result.Value)
                    : Results.NotFound(result.Error);
            });

            orders.MapPost("/{orderId:guid}/confirm", async (Guid orderId, IMediator mediator) =>
            {
                var command = new ConfirmOrderCommand(orderId);
                var result = await mediator.Send(command);

                return result.IsSuccess 
                    ? Results.Ok()
                    : Results.BadRequest(result.Error);
            });

            // Inventory Context Endpoints
            var inventory = v1.MapGroup("/inventory").WithTags("Inventory");

            inventory.MapGet("/{productId:guid}", async (Guid productId, IMediator mediator) =>
            {
                var query = new GetInventoryQuery(productId);
                var result = await mediator.Send(query);

                return result.IsSuccess 
                    ? Results.Ok(result.Value)
                    : Results.NotFound(result.Error);
            });

            inventory.MapPost("/reserve", async ([FromBody] ReserveInventoryRequest request, IMediator mediator) =>
            {
                var command = new ReserveInventoryCommand(request.OrderId, request.Items);
                var result = await mediator.Send(command);

                return result.IsSuccess 
                    ? Results.Ok()
                    : Results.BadRequest(result.Error);
            });

            app.Run();
        }
    }

    // Request DTOs
    public record CreateOrderRequest(Guid UserId, List<OrderItemRequest> Items);
    public record ReserveInventoryRequest(Guid OrderId, List<ReservationRequest> Items);
}
