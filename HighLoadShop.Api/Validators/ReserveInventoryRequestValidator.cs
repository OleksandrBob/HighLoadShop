using FluentValidation;
using HighLoadShop.Api.Models;

namespace HighLoadShop.Api.Validators;

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
            .SetValidator(new ReservationRequestValidator());
    }
}

public class ReservationRequestValidator : AbstractValidator<HighLoadShop.Application.InventoryContext.Commands.ReserveInventory.ReservationRequest>
{
    public ReservationRequestValidator()
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
