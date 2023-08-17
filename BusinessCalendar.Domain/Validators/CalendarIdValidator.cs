using BusinessCalendar.Domain.Dto;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class CalendarIdValidator : AbstractValidator<CalendarId>
{
    public CalendarIdValidator()
    {
        RuleFor(x => x.Key)
                .Matches(@"^[A-z_]*$")
            .WithMessage("{PropertyName} value must match regex {RegularExpression}");

        RuleFor(x => x.Year).InclusiveBetween(DateTime.MinValue.Year, DateTime.MaxValue.Year)
            .WithMessage("{PropertyName} value must be in a range between {From} and {To}");
    }
}