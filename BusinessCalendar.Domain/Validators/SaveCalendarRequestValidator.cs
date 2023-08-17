using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class SaveCalendarRequestValidator : AbstractValidator<SaveCalendarRequest>
{
    public SaveCalendarRequestValidator(CalendarIdValidator calendarIdValidator)
    {
        RuleFor(x => new CalendarId { Type = x.Type, Key = x.Key, Year = x.Year })
            .SetValidator(calendarIdValidator)
            .DependentRules(() =>
                RuleForEach(x => x.Dates)
                    .Must((calendar, day) => day.Date.Year == calendar.Year)
                    .WithMessage($"Each date must be part of the year"));
    }
}