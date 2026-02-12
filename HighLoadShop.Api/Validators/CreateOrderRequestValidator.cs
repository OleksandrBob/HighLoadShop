using FluentValidation;
using HighLoadShop.Api.Models;

namespace HighLoadShop.Api.Validators;

public class CreateOrderRequestValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item.")
            .Must(items => items.Count > 0)
            .WithMessage("Order must contain at least one item.");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemRequestValidator());
    }
}

public class OrderItemRequestValidator : AbstractValidator<Application.OrderContext.Commands.CreateOrder.OrderItemRequest>
{
    public OrderItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0.")
            .LessThanOrEqualTo(1000)
            .WithMessage("Quantity cannot exceed 1000.");
    }
}
