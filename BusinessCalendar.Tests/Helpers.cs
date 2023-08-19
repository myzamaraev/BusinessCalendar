using FluentValidation;

namespace BusinessCalendar.Tests;

internal static class Helpers
{
    public static IValidator<T> GetFakeFailureValidator<T>()
    {
        
        return new InlineValidator<T>()
        {
            v => v.RuleFor(x => x).Must(x => false).WithMessage("Failure")
        };
    }
}