using System.Linq.Expressions;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Providers;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class SaveCompactCalendarRequestValidator : AbstractValidator<SaveCompactCalendarRequest>
{
    public SaveCompactCalendarRequestValidator(CalendarIdValidator calendarIdValidator)
    {
        RuleFor(x => new CalendarId { Type = x.Type, Key = x.Key, Year = x.Year })
            .SetValidator(calendarIdValidator)
            .DependentRules(() =>
            {
                RuleFor(x => x.Holidays)
                    .Must((request, holidays) => EveryDateComplyWith(request.Year, holidays, x => x.IsWorkday))
                    .WithMessage("Each date in {PropertyName} must be a workday");
                
                RuleFor(x => x.ExtraWorkDays)
                    .Must((request, extraWorkDays) => EveryDateComplyWith(request.Year, extraWorkDays, x => !x.IsWorkday))
                    .WithMessage("Each date in {PropertyName} must be a weekend");
            } );
    }

    private static bool EveryDateComplyWith(int year, List<DateOnly> dates, Func<CalendarDate, bool> predicate)
    {
        var defaultCalendar = DefaultCalendarProvider.DefaultCalendar(year).ToList();
        return dates.All(date => predicate(defaultCalendar.Single(day => day.Date == date)));
    }
}