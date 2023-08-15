using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using BusinessCalendar.Domain.Exceptions;
using Hellang.Middleware.ProblemDetails;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ProblemDetailsOptionsExtensions
{
    public static void MapFluentValidationException(this ProblemDetailsOptions options, int? statusCode = null) =>
        options.Map<ValidationException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
    
            var errors = ex.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(x => x.ErrorMessage).ToArray());
    
            return factory.CreateValidationProblemDetails(ctx, errors, statusCode);
        });
}
