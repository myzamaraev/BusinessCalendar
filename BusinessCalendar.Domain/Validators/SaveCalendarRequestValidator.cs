using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class SaveCalendarRequestValidator : AbstractValidator<SaveCalendarRequest>
{
    public SaveCalendarRequestValidator(IValidator<CalendarId> calendarIdValidator)
    {
        RuleFor(x => new CalendarId { Type = x.Type, Key = x.Key, Year = x.Year })
            .SetValidator(calendarIdValidator)
            .DependentRules(() =>
            {
                RuleForEach(x => x.Dates)
                    .Must((calendar, day) => day.Date.Year == calendar.Year)
                    .WithMessage((calendar, day) => $"Date {day.Date} has year different from {calendar.Year}"); 
                
                RuleFor(x => x.Dates.Select(calendarDate => calendarDate.Date))
                    .Must(days => days.Distinct().Count() == days.Count())
                    .WithMessage($"Dates array has duplicates");
            } );
    }
}