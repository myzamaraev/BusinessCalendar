using BusinessCalendar.Domain.Dto.Requests;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class SaveCalendarRequestValidator : AbstractValidator<SaveCalendarRequest>
{
    public SaveCalendarRequestValidator()
    {
        RuleFor(x => x.Key)
            .Matches(@"^[\w]*$")
            .WithMessage("Key value must match regex {RegularExpression}");
        
        RuleFor(x => x.Year).InclusiveBetween(DateTime.MinValue.Year, DateTime.MaxValue.Year)
            .WithMessage($"Year value must be in a range between {DateTime.MinValue.Year} and {DateTime.MaxValue.Year}");
        
        RuleForEach(x => x.Dates)
            .Must((calendar, day) => day.Date.Year == calendar.Year)
            .WithMessage($"Each date must be part of the year");
    }
}