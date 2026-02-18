using FluentValidation;
using InventoryService.Api.Models;
using InventoryService.Application.Commands.ReserveInventory;

namespace InventoryService.Api.Validators;

public class ReserveInventoryRequestValidator : AbstractValidator<ReserveInventoryRequest>
{
    public ReserveInventoryRequestValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("OrderId is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Reservation must contain at least one item.")
            .Must(items => items.Count > 0)
            .WithMessage("Reservation must contain at least one item.");

        RuleForEach(x => x.Items)
            .SetValidator(new ReservationItemRequestValidator());
    }
}

public class ReservationItemRequestValidator : AbstractValidator<ReservationItemRequest>
{
    public ReservationItemRequestValidator()
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
