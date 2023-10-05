using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Services;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class CalendarIdValidator : AbstractValidator<CalendarId>
{
    public CalendarIdValidator(ICalendarIdentifierService calendarIdentifierService)
    {
        RuleFor(x => x)
            .MustAsync(async (request, cancellationToken) =>
            {
                var identifier = new CalendarIdentifier(request.Type, request.Key);
                return await calendarIdentifierService.GetAsync(identifier.Id, cancellationToken) != null;
            })
            .WithMessage((x) => $"No calendar identifier found for Type: {x.Type.ToString()}, Key: {x.Key}");

        RuleFor(x => x.Year).InclusiveBetween(DateTime.MinValue.Year, DateTime.MaxValue.Year)
            .WithMessage("{PropertyName} value must be in a range between {From} and {To}");
    }
}