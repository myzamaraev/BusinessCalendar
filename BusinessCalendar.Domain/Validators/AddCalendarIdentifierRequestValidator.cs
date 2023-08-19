using BusinessCalendar.Domain.Dto.Requests;
using FluentValidation;

namespace BusinessCalendar.Domain.Validators;

public class AddCalendarIdentifierRequestValidator : AbstractValidator<AddCalendarIdentifierRequest>
{
    public AddCalendarIdentifierRequestValidator()
    {
        // if (string.IsNullOrWhiteSpace(request.Key))
        // {
        //     throw new ArgumentOutOfRangeException(nameof(request.Key), "must not be null, empty, or whitespace string");
        // }
    }
}