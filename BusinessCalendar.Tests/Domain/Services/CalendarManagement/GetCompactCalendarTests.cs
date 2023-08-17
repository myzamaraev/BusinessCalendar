using BusinessCalendar.Domain.Dto;
using FluentAssertions;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

public partial class CalendarManagementServiceTests
{
    [Test]
    public async Task Should_GetCompactCalendarAsync_call_storage_FindOne()
    {
        var calendarId = DefaultCalendarId;

        await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);
        
        _calendarStorageServiceMock.Verify(x => x.FindOne(It.Is<CalendarId>(id => id == calendarId)));
    }

    [Test]
    public async Task Should_GetCompactCalendarAsync_return_compactCalendar()
    {
        var calendarId = DefaultCalendarId;
        var expected = new CompactCalendar(new Calendar(calendarId));
        
        _calendarStorageServiceMock.Setup(x => x.FindOne(It.IsAny<CalendarId>()))
            .ReturnsAsync(expected);
        
        var actual = await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);

        actual.Should().NotBeNull("we expect the Calendar from storage to be returned");
        actual.Should().BeSameAs(expected, Constants.ShouldReferenceTheSameObject);
    }

    [Test]
    public async Task Should_GetCompactCalendarAsync_return_DefaultCalendar_when_no_match_in_storage()
    {
        var calendarId = DefaultCalendarId;
        
        var actual = await CreateCalendarManagementService().GetCompactCalendarAsync(calendarId);
        
        
        actual.Should().NotBeNull("we expect the default Calendar to be created");
        
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.SameAs(calendarId));
            Assert.That(actual.IsDefault, Is.True);
        });
    }
}