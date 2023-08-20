using BusinessCalendar.Domain.Dto.Requests;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class AddCalendarIdentifierRequestValidator : AbstractValidator<AddCalendarIdentifierRequest>
{
    public AddCalendarIdentifierRequestValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .Matches(@"^[A-z_]*$")
            .WithMessage("{PropertyName} value must match regex {RegularExpression}");
    }
}