using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

public partial class CalendarManagementServiceTests
{
    [Test]
    public async Task Should_GetCompactCalendarAsync_call_storage_FindOne()
    {
        var calendarId = DefaultCalendarId;

        await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);
        
        _calendarStorageServiceMock.Verify(x => x.FindOneAsync(
            It.Is<CalendarId>(id => id == calendarId),
            It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_GetCompactCalendarAsync_return_compactCalendar()
    {
        var calendarId = DefaultCalendarId;
        var expected = new CompactCalendar(new Calendar(calendarId));
        
        _calendarStorageServiceMock.Setup(x => x.FindOneAsync(It.IsAny<CalendarId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        
        var actual = await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);

        actual.Should().NotBeNull("we expect the Calendar from storage to be returned");
        actual.Should().BeSameAs(expected, Constants.ShouldReferenceTheSameObject);
    }

    [Test]
    public async Task Should_GetCompactCalendarAsync_return_DefaultCalendar_when_no_match_in_storage()
    {
        var calendarId = DefaultCalendarId;
        var expected = new CompactCalendar(new Calendar(calendarId));
        
        var actual = await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);
        
        
        actual.Should().NotBeNull("we expect the default Calendar to be created");
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task Should_GetCompactCalendar_call_CalendarIdValidator_once()
    {
        var calendarId = new CalendarId() { Type = CalendarType.Custom, Key = "Test", Year = Constants.CurrentYear};
        
        var actual = await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);
        
        _calendarIdValidatorMock.Verify(x => 
                x.ValidateAsync(
                    It.Is<ValidationContext<CalendarId>>(context => context.InstanceToValidate == calendarId && context.ThrowOnFailures == true), 
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test]
    public async Task Should_GetCompactCalendar_throw_when_CalendarIdValidator_failed()
    {
        var fakeValidator = TestValidator.Faulty<CalendarId>();
        var calendarId = new CalendarId() { Type = CalendarType.Custom, Key = "Test", Year = Constants.CurrentYear};
        
        var actual = await CreateCalendarManagementService(calendarIdValidator: fakeValidator)
            .Invoking(x => x.GetCompactCalendarAsync(calendarId))
            .Should()
            .ThrowExactlyAsync<ValidationException>();

        actual.Subject.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().Contain(failure => failure.ErrorMessage == "Failure");
    }
}