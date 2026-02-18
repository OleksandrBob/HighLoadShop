using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace InventoryService.Api.Filters;

public class ValidationActionFilter<T> : IAsyncActionFilter where T : class
{
    private readonly IValidator<T>? _validator;

    public ValidationActionFilter(IServiceProvider serviceProvider)
    {
        _validator = serviceProvider.GetService<IValidator<T>>();
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (_validator is null)
        {
            await next();
            return;
        }

        var requestObject = context.ActionArguments.Values.OfType<T>().FirstOrDefault();

        if (requestObject is null)
        {
            await next();
            return;
        }

        var validationResult = await _validator.ValidateAsync(requestObject, context.HttpContext.RequestAborted);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.ToDictionary();
            context.Result = new BadRequestObjectResult(new ValidationProblemDetails(errors));
            return;
        }

        await next();
    }
}
