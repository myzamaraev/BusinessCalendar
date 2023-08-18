using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;

namespace BusinessCalendar.Tests.Domain.Validators;

[TestFixture]
public class CalendarIdValidatorTests
{
    
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    [TestCase("  ")]
    public void Should_return_error_when_key_is_empty(string key)
    {
        var calendarId = new CalendarId() { Key = key };
        var validator = new CalendarIdValidator();
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
        var calendarId = new CalendarId() { Key = key };
        var validator = new CalendarIdValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "Key value must match regex ^[A-z_]*$");
    }

    [TestCase("abc")]
    [TestCase("abc_bca")]
    public void Should_not_return_error_when_key_matches_regex(string key)
    {
        var calendarId = new CalendarId { Key = key };
        var validator = new CalendarIdValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.Errors.Should().NotContain(x => x.ErrorMessage == "Key value must match regex ^[A-z_]*$");
    }

    [TestCase(-2000)]
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(10_000)]
    public void Should_return_error_when_year_out_of_range(int year)
    {
        var calendarId = new CalendarId { Year = year };
        var validator = new CalendarIdValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "Year value must be in a range between 1 and 9999");
    }
    
    [TestCase(1)]
    [TestCase(200)]
    [TestCase(2035)]
    [TestCase(9999)]
    public void Should_not_return_error_when_year_inside_allowed_range(int year)
    {
        var calendarId = new CalendarId { Year = year };
        var validator = new CalendarIdValidator();
        var result = validator.Validate(calendarId);

        result.Should().NotBeNull();
        result.Errors.Should().NotContain(x => x.ErrorMessage == "Year value must be in a range between 1 and 9999");
    }
}