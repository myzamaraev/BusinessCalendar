using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;
using Moq;

namespace BusinessCalendar.Tests.Domain.Validators;

[TestFixture]
public class CalendarIdValidatorTests
{
    private Mock<ICalendarIdentifierService> _calendarIdentifierServiceMock;

    [SetUp]
    public void SetUp()
    {
        _calendarIdentifierServiceMock = new Mock<ICalendarIdentifierService>();
    }

    [Test]
    public async Task Should_return_error_when_no_identifier_found()
    {
        var calendarId = new CalendarId(CalendarType.Custom, "Test", Constants.CurrentYear);
        var validator = new CalendarIdValidator(_calendarIdentifierServiceMock.Object);
        var result = await validator.ValidateAsync(calendarId);
        
        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "No calendar identifier found for Type: Custom, Key: Test");
    }

    [Test]
    public async Task Should_not_return_error_when_identifier_found()
    {
        var calendarId = new CalendarId(CalendarType.Custom, "Test", Constants.CurrentYear);
        _calendarIdentifierServiceMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CalendarIdentifier(CalendarType.Custom, "Test"));
        
        var validator = new CalendarIdValidator(_calendarIdentifierServiceMock.Object);
        var result = await validator.ValidateAsync(calendarId);
        
        result.Should().NotBeNull();
        result.Errors.Should().NotContain(x => x.ErrorMessage == "No calendar identifier found for Type: Custom, Key: Test");
        _calendarIdentifierServiceMock.Verify(x =>
                x.GetAsync(It.Is<string>(id => id =="Custom_Test"), It.IsAny<CancellationToken>())
            , Times.Once);
    }

    [TestCase(-2000)]
    [TestCase(-1)]
    [TestCase(0)]
    [TestCase(10_000)]
    public async Task Should_return_error_when_year_out_of_range(int year)
    {
        var calendarId = new CalendarId(CalendarType.Custom, "Test",  year);
        var validator = new CalendarIdValidator(_calendarIdentifierServiceMock.Object);
        var result = await validator.ValidateAsync(calendarId);


        result.Should().NotBeNull();
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == "Year value must be in a range between 1 and 9999");
    }
    
    [TestCase(1)]
    [TestCase(200)]
    [TestCase(2035)]
    [TestCase(9999)]
    public async Task Should_not_return_error_when_year_inside_allowed_range(int year)
    {
        var calendarId = new CalendarId(CalendarType.Custom, "Test",  year);
        var validator = new CalendarIdValidator(_calendarIdentifierServiceMock.Object);
        var result = await validator.ValidateAsync(calendarId);

        result.Should().NotBeNull();
        result.Errors.Should().NotContain(x => x.ErrorMessage == "Year value must be in a range between 1 and 9999");
    }
}