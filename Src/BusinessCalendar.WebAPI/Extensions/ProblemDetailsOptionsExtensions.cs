using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.Domain.Extensions;
using Hellang.Middleware.ProblemDetails;
using ProblemDetailsOptions = Hellang.Middleware.ProblemDetails.ProblemDetailsOptions;

namespace BusinessCalendar.WebAPI.Extensions;

public static class ProblemDetailsOptionsExtensions
{
    /// <summary>
    /// Maps FluentValidation.ValidationException to problem details
    /// </summary>
    /// <param name="options"></param>
    /// <param name="statusCode">desired http status code</param>
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
    
    /// <summary>
    /// Maps any exception derived from ClientException to problem details
    /// </summary>
    /// <param name="options"></param>
    /// <param name="statusCode">desired http status code</param>
    public static void MapClientException(this ProblemDetailsOptions options, int? statusCode = null) =>
        options.Map<ClientException>((ctx, ex) =>
        {
            var factory = ctx.RequestServices.GetRequiredService<ProblemDetailsFactory>();
            var problemDetails = factory.CreateProblemDetails(ctx, statusCode, title: ex.ErrorCode.GetDescription(), detail: ex.Message);
            if (ex.Details != null)
            {
                problemDetails.Extensions.Add(nameof(ex.Details), ex.Details);
            }

            return problemDetails;
        });
}
