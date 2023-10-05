using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Mappers;
using BusinessCalendar.Domain.Providers;
using FluentAssertions;

namespace BusinessCalendar.Tests.Domain.Mappers;

public class CalendarMapperTests
{
    [Test, TestCaseSource(nameof(CalendarDatesTestCaseSource))]
    public void Should_Map_CompactCalendar_to_Calendar(List<CalendarDate> calendarDates)
    {
        var calendarId = new CalendarId() { Year = Constants.CurrentYear };
        var expected = new Calendar(calendarId, calendarDates);
        var compactCalendar = new CompactCalendar(expected);

        var actual = CreateCalendarMapper().Map(compactCalendar);

        actual.Should().NotBeNull("should always return full Calendar");
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test, TestCaseSource(nameof(CalendarDatesTestCaseSource))]
    public void Should_MapToCompact_map_Calendar_to_CompactCalendar(List<CalendarDate> calendarDates)
    {
        var calendarId = new CalendarId() { Year = Constants.CurrentYear };
        var calendar = new Calendar(calendarId, calendarDates);
        var expected = new CompactCalendar(calendar);

        var actual = CreateCalendarMapper().MapToCompact(calendar);

        actual.Should().NotBeNull("should always return full Calendar");
        actual.Should().BeEquivalentTo(expected);
    }

    [Test, TestCaseSource(nameof(CalendarDatesTestCaseSource))]
    public void Should_MapToCompact_map_SaveCalendarRequest_to_CompactCalendar(List<CalendarDate> calendarDates)
    {
        var saveCalendarRequest = new SaveCalendarRequest
        {
            Type = CalendarType.State,
            Key = "Test",
            Year = Constants.CurrentYear,
            Dates = calendarDates
        };

        var calendar = new Calendar(new CalendarId()
        {
            Type = CalendarType.State,
            Key = "Test",
            Year = Constants.CurrentYear
        }, calendarDates);

        var expected = new CompactCalendar(calendar);
        
        var actual = CreateCalendarMapper().MapToCompact(saveCalendarRequest);

        actual.Should().NotBeNull("should always return CompactCalendar");
        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test, TestCaseSource(nameof(CalendarDatesTestCaseSource))]
    public void Should_MapToCompact_map_SaveCompactCalendarRequest_to_CompactCalendar(List<CalendarDate> calendarDates)
    {
        var calendar = new Calendar(new CalendarId()
        {
            Type = CalendarType.State,
            Key = "Test",
            Year = Constants.CurrentYear
        }, calendarDates);
        
        var expected = new CompactCalendar(calendar);
        
        var saveCompactCalendarRequest = new SaveCompactCalendarRequest()
        {
            Type = CalendarType.State,
            Key = "Test",
            Year = Constants.CurrentYear,
            Holidays = { expected.Holidays },
            ExtraWorkDays = { expected.ExtraWorkDays }
        };

        var actual = CreateCalendarMapper().MapToCompact(saveCompactCalendarRequest);

        actual.Should().NotBeNull("should always return CompactCalendar");
        actual.Should().BeEquivalentTo(expected);
    }
    
    private static IEnumerable<List<CalendarDate>> CalendarDatesTestCaseSource()
    {
        var defaultYearDates = DefaultCalendarProvider.DefaultCalendar(DateTime.Today.Year).ToList();
        yield return defaultYearDates;
        
        var invertedYearDates = defaultYearDates
            .Select(x => x with { IsWorkday = !x.IsWorkday })
            .ToList();
        yield return invertedYearDates;
    }

    private CalendarMapper CreateCalendarMapper()
    {
        return new CalendarMapper();
    }


    private DateOnly GetFirstWorkdayOfYear(int year)
    {
        var date = new DateOnly(year, 1, 1);
        while (date.IsWeekend())
        {
            date = date.AddDays(1);
        }

        return date;
    }

    private DateOnly GetFirstWeekendOfYear(int year)
    {
        var date = new DateOnly(year, 1, 1);
        while (!date.IsWeekend())
        {
            date = date.AddDays(1);
        }

        return date;
    }
}