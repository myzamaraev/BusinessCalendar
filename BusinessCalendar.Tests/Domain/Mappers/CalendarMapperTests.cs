using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Mappers;

namespace BusinessCalendar.Tests.Domain.Mappers;

public class CalendarMapperTests
{
    [Test]
    public void Should_GetCalendarAsync_return_full_version_of_CompactCalendar_from_storage()
    {
        var year = DateTime.Today.Year;
        var weekend = GetFirstWeekendOfYear(year);
        var workday = GetFirstWorkdayOfYear(year);
        
        //todo: replace with inverse version of DefaultCalendar in compact form
        var compactCalendar = new CompactCalendar(new Calendar(new CalendarId() {Year = year}))
        {
            ExtraWorkDays = { weekend },
            Holidays = { workday }
        };

        var actual = CreateCalendarMapper().Map(compactCalendar);
        
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.Id, Is.SameAs(compactCalendar.Id));
            Assert.That(actual.Dates.First(x => x.Date == weekend).IsWorkday, Is.True);
            Assert.That(actual.Dates.First(x => x.Date == workday).IsWorkday, Is.False);
        });
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