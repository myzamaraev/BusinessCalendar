using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;

namespace BusinessCalendar.Tests.Domain.Validators;

public class AddCalendarIdentifierRequestValidatorTests
{
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("  ")]
    public void Should_return_error_when_key_is_empty(string key)
    {
        var calendarId = new AddCalendarIdentifierRequest() { Key = key };
        
        var validator = new AddCalendarIdentifierRequestValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "'Key' must not be empty.");
    }
    
    [TestCase("abc123")]
    [TestCase("abc-bca")]
    [TestCase("abc*")]
    [TestCase("abc@")]
    public void Should_return_error_when_key_not_matches_regex(string key)
    {
        var calendarId = new AddCalendarIdentifierRequest() { Key = key };
        
        var validator = new AddCalendarIdentifierRequestValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "Key value must match regex ^[A-z_]*$");
    }

    [TestCase("abc")]
    [TestCase("abc_bca")]
    public void Should_not_return_error_when_key_matches_regex(string key)
    {
        var calendarId = new AddCalendarIdentifierRequest() { Key = key };
        
        var validator = new AddCalendarIdentifierRequestValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.Errors.Should().NotContain(x => x.ErrorMessage == "Key value must match regex ^[A-z_]*$");
    }
}