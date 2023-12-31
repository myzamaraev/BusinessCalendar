using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Validators;

[TestFixture]
public class CompactCalendarValidatorTests
{
    private Mock<CalendarIdValidator> _calendarIdValidatorMock;

    [SetUp]
    public void SetUp()
    {
        var calendarIdentifierServiceMock = new Mock<ICalendarIdentifierService>();
        _calendarIdValidatorMock = new Mock<CalendarIdValidator>(calendarIdentifierServiceMock.Object);
    }

    [Test]
    public void Should_call_CalendarIdValidator()
    {
        var compactCalendar = new CompactCalendar(new CalendarId());

        var validator = new CompactCalendarValidator(_calendarIdValidatorMock.Object);

        validator.Validate(compactCalendar);

        _calendarIdValidatorMock.Verify(x =>
                x.Validate(It.Is<ValidationContext<CalendarId>>(context =>
                    context.InstanceToValidate == compactCalendar.Id)),
            Times.Once);
    }

    [Test]
    public void Should_fail_when_duplicateDates()
    {
        var compactCalendar = new CompactCalendar(new CalendarId() { Year = Constants.CurrentYear })
        {
            Holidays =
            {
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today)
            },
            ExtraWorkDays =
            {
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today)
            }
        };

        var validator = new CompactCalendarValidator(_calendarIdValidatorMock.Object);

        var result = validator.Validate(compactCalendar);

        result.Should().NotBeNull();
        result.Errors.Should().Contain(failure => failure.ErrorMessage == "Holidays array has duplicate dates");
        result.Errors.Should().Contain(failure => failure.ErrorMessage == "ExtraWorkDays array has duplicate dates");
    }
    
    [Test]
    public void Should_fail_when_date_out_of_year()
    {
        var startOfNextYear = new DateOnly(Constants.CurrentYear + 1, 1, 1);
        var endOfPrevYear = new DateOnly(Constants.CurrentYear - 1, 12, 31);
        
        var compactCalendar = new CompactCalendar(new CalendarId() { Year = Constants.CurrentYear })
        {
            Holidays = { startOfNextYear },
            ExtraWorkDays = { endOfPrevYear }
        };

        var validator = new CompactCalendarValidator(_calendarIdValidatorMock.Object);

        var result = validator.Validate(compactCalendar);

        result.Should().NotBeNull();
        result.Errors.Should().Contain(failure => failure.ErrorMessage == $"Holidays: Date {startOfNextYear} has year different from {Constants.CurrentYear}");
        result.Errors.Should().Contain(failure => failure.ErrorMessage == $"ExtraWorkDays: Date {endOfPrevYear} has year different from {Constants.CurrentYear}");
    }
    
    [Test]
    public void Should_fail_when_date_has_same_default_value()
    {
        var firstDayOfYear = new DateOnly(2023, 1, 1);
        var secondDayOfYear = new DateOnly(2023, 1, 2);
        var compactCalendar = new CompactCalendar(new CalendarId() { Year = Constants.CurrentYear })
        {
            Holidays = { firstDayOfYear },
            ExtraWorkDays = { secondDayOfYear }
        };

        var validator = new CompactCalendarValidator(_calendarIdValidatorMock.Object);

        var result = validator.Validate(compactCalendar);

        result.Should().NotBeNull();
        result.Errors.Should().Contain(failure => failure.ErrorMessage == $"Holidays: Date {firstDayOfYear} is weekend by default.");
        result.Errors.Should().Contain(failure => failure.ErrorMessage == $"ExtraWorkDays: Date {secondDayOfYear} is workday by default.");
    }
    
    [Test]
    public void Should_fail_when_CalendarIdValidator_failed()
    {
        var compactCalendar = new CompactCalendar(new CalendarId())
        {
            //duplicate dates should not cause additional errors in this scenario because this rule should depend on CalendarId validation result
            Holidays =
            {
                DateOnly.FromDateTime(DateTime.Today),
                DateOnly.FromDateTime(DateTime.Today)
            }
        };

        var validator = new CompactCalendarValidator(TestValidator.Faulty<CalendarId>());

        var result = validator.Validate(compactCalendar);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(failure => failure.ErrorMessage == "Failure");
    }
}