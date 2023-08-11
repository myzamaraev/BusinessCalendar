using System.Linq.Expressions;
using BusinessCalendar.Domain.Dto;
using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace BusinessCalendar.Domain.Validators;

public class CompactCalendarValidator : AbstractValidator<CompactCalendar>
{
    public CompactCalendarValidator()
    {
        AddDateValidationRules(x => x.Holidays);
        AddDateValidationRules(x => x.ExtraWorkDays);
    }
    
    private void AddDateValidationRules(Expression<Func<CompactCalendar, IEnumerable<DateOnly>>> expression)
    {
        var expressionBody = (MemberExpression)expression.Body;
        var propName = expressionBody.Member.Name;

        RuleForEach(expression)
            .Must((calendar, date) => date.Year == calendar.Id.Year)
            .WithMessage($"Each date in {propName} must be part of the year");

        RuleFor(expression)
            .Must(days => days.Distinct().Count() == days.Count())
            .WithMessage($"{propName} array has duplicate dates");
    }
}