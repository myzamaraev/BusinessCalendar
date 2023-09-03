using BusinessCalendar.Domain.Validators;
using FluentValidation;
using FluentValidation.Validators;

namespace BusinessCalendar.Tests;

internal static class TestValidator
{
    public static InlineValidator<T> Faulty<T>() => new ()
    {
        v => v.RuleFor(x => x).Must(x => false).WithMessage("Failure")
    };
}