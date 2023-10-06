using System.Linq.Expressions;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Extensions;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class CompactCalendarValidator : AbstractValidator<CompactCalendar>
{
    public CompactCalendarValidator(IValidator<CalendarId> calendarIdValidator)
    {
        RuleFor(x => x.Id)
            .SetValidator(calendarIdValidator)
            .DependentRules(() =>
            {
                AddDateValidationRules(x => x.Holidays, DateMustBe.Workday);
                AddDateValidationRules(x => x.ExtraWorkDays, DateMustBe.Weekend);
            });
    }

    private void AddDateValidationRules(Expression<Func<CompactCalendar, IEnumerable<DateOnly>>> expression, DateMustBe dateMustBe)
    {
        var expressionBody = (MemberExpression)expression.Body;
        var propName = expressionBody.Member.Name;
        
        RuleForEach(expression)
            .Must((calendar, date) => date.Year == calendar.Id.Year)
            .WithMessage((calendar, date) => $"{propName}: Date {date} has year different from {calendar.Id.Year}")
            //Check every item in Holidays is workday
            .Must(date => !date.IsWeekend())
            .When(_ => dateMustBe == DateMustBe.Workday, ApplyConditionTo.CurrentValidator)
            .WithMessage((_, date) => $"{propName}: Date {date} is {GetWeekdayDescription(date)} by default.")
            //Check every item in ExtraWorkdays is weekend
            .Must(date => date.IsWeekend())
            .When(_ => dateMustBe == DateMustBe.Weekend, ApplyConditionTo.CurrentValidator)
            .WithMessage((_, date) => $"{propName}: Date {date} is {GetWeekdayDescription(date)} by default.");

        RuleFor(expression)
            .Must(days => days.Distinct().Count() == days.Count())
            .WithMessage($"{propName} array has duplicate dates");
    }

    private static string GetWeekdayDescription(DateOnly date) => date.IsWeekend() ? "weekend" : "workday";
    
    private enum DateMustBe : byte
    {
        Workday = 0,
        Weekend = 1
    }
}

