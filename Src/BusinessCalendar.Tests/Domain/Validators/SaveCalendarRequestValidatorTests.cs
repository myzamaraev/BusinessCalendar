using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Validators;

[TestFixture]
public class SaveCalendarRequestValidatorTests
{
    private Mock<CalendarIdValidator> _calendarIdValidatorMock;

    [SetUp]
    public void SetUp()
    {
        var calendarIdentifierServiceMock = new Mock<ICalendarIdentifierService>();
        _calendarIdValidatorMock = new Mock<CalendarIdValidator>(calendarIdentifierServiceMock.Object);
    }

    [Test]
    public void Should_fail_when_date_out_of_year()
    {
        var startOfNextYear = new DateOnly(Constants.CurrentYear + 1, 1, 1);
        var endOfPrevYear = new DateOnly(Constants.CurrentYear - 1, 12, 31);
        
        var compactCalendar = new SaveCalendarRequest()
        {
            Year = Constants.CurrentYear,
            Dates =
            {
                new CalendarDate { Date = startOfNextYear },
                new CalendarDate { Date = endOfPrevYear },
            }
        };

        var validator = new SaveCalendarRequestValidator(_calendarIdValidatorMock.Object);

        var result = validator.Validate(compactCalendar);

        result.Should().NotBeNull();
        result.Errors.Should().Contain(failure =>
            failure.ErrorMessage == $"Date {startOfNextYear} has year different from {Constants.CurrentYear}");
        result.Errors.Should().Contain(failure =>
            failure.ErrorMessage == $"Date {endOfPrevYear} has year different from {Constants.CurrentYear}");
    }
    
    [Test]
    public void Should_fail_when_duplicateDates()
    {
        var compactCalendar = new SaveCalendarRequest()
        {
            Dates =
            {
                new CalendarDate { Date = new DateOnly(Constants.CurrentYear , 1, 1) },
                new CalendarDate { Date = new DateOnly(Constants.CurrentYear, 1, 1) },
            }
        };

        var validator = new SaveCalendarRequestValidator(_calendarIdValidatorMock.Object);

        var result = validator.Validate(compactCalendar);

        result.Should().NotBeNull();
        result.Errors.Should().Contain(failure => failure.ErrorMessage == "Dates array has duplicates");
    }

    [Test]
    public void Should_call_CalendarIdValidator()
    {
        var request = new SaveCalendarRequest()
        {
            Type = CalendarType.Custom,
            Key = "Test",
            Year = Constants.CurrentYear
        };

        var validator = new SaveCalendarRequestValidator(_calendarIdValidatorMock.Object);

        validator.Validate(request);
        
        _calendarIdValidatorMock.Verify(x =>
                x.Validate(It.Is<ValidationContext<CalendarId>>(context =>
                    context.InstanceToValidate.Type == request.Type
                    && context.InstanceToValidate.Key == request.Key
                    && context.InstanceToValidate.Year == request.Year)),
            Times.Once);
    }
    
    [Test]
    public void Should_fail_when_CalendarIdValidator_failed()
    {
        var saveCalendarRequest = new SaveCalendarRequest();

        var validator = new SaveCalendarRequestValidator(TestValidator.Faulty<CalendarId>());

        var result = validator.Validate(saveCalendarRequest);

        result.Should().NotBeNull();
        result.Errors.Should().HaveCount(1);
        result.Errors.Should().Contain(failure => failure.ErrorMessage == "Failure");
    }
    
}