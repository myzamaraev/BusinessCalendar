using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

public partial class CalendarManagementServiceTests
{
    [Test]
    public async Task Should_GetCalendarAsync_call_storage_FindOne()
    {
        var calendarId = DefaultCalendarId;

        await CreateCalendarManagementService().GetCalendarAsync(calendarId);

        _calendarStorageServiceMock.Verify(x => x.FindOneAsync(
            It.Is<CalendarId>(id => id == calendarId),
            It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_GetCalendarAsync_call_mapper_from_CompactCalendar_to_Calendar()
    {
        var calendarId = DefaultCalendarId;

        var expected = new CompactCalendar(new Calendar(calendarId));
        _calendarStorageServiceMock.Setup(x => x.FindOneAsync(It.IsAny<CalendarId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);

        var actual = await CreateCalendarManagementService().GetCalendarAsync(calendarId);

        _calendarMapper.Verify(x => x.Map(It.Is<CompactCalendar>(c => c == expected)), Times.Once);
    }

    [Test]
    public async Task Should_GetCalendarAsync_return_mapping_result()
    {
        var calendarId = DefaultCalendarId;
        var compactCalendar = new CompactCalendar(new Calendar(calendarId));
        var expected = new Calendar(calendarId);

        _calendarStorageServiceMock.Setup(x => x.FindOneAsync(It.IsAny<CalendarId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(compactCalendar); //mock storage to pass null check

        _calendarMapper.Setup(x => x.Map(It.IsAny<CompactCalendar>()))
            .Returns(expected);

        var actual = await CreateCalendarManagementService().GetCalendarAsync(calendarId);

        actual.Should().NotBeNull("we expect the mapped Calendar to be returned");
        actual.Should().BeSameAs(expected, Constants.ShouldReferenceTheSameObject);
    }

    [Test]
    public async Task Should_GeCalendarAsync_return_default_Calendar_when_no_match_in_storage()
    {
        var calendarId = DefaultCalendarId;
        var expected = new Calendar(calendarId);

        var actual = await CreateCalendarManagementService().GetCalendarAsync(calendarId);

        actual.Should().NotBeNull("we expect the default Calendar to be created");
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task Should_GetCalendar_call_CalendarIdValidator_once()
    {
        var calendarId = new CalendarId() { Type = CalendarType.Custom, Key = "Test", Year = Constants.CurrentYear};
        
        var actual = await CreateCalendarManagementService().GetCalendarAsync(calendarId);
        
        _calendarIdValidatorMock.Verify(x => 
            x.ValidateAsync(
                It.Is<ValidationContext<CalendarId>>(context => context.InstanceToValidate == calendarId && context.ThrowOnFailures == true), 
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test]
    public async Task Should_GetCalendar_throw_when_CalendarIdValidator_failed()
    {
        var fakeValidator = TestValidator.Faulty<CalendarId>();
        var calendarId = new CalendarId() { Type = CalendarType.Custom, Key = "Test", Year = Constants.CurrentYear};
        
        var actual = await CreateCalendarManagementService(calendarIdValidator: fakeValidator)
            .Invoking(x => x.GetCalendarAsync(calendarId))
            .Should()
            .ThrowExactlyAsync<ValidationException>();

        actual.Subject.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().Contain(failure => failure.ErrorMessage == "Failure");
    }
}