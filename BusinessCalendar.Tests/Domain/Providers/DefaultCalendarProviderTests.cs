using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Providers;
using FluentAssertions;

namespace BusinessCalendar.Tests.Domain.Providers;

[TestFixture]
public class DefaultCalendarProviderTests
{
    [Test]
    public void Should_provide_correct_first_week()
    {
        const int year = 2023;
        var expected = new List<CalendarDate>
        {
            new() { Date = new DateOnly(year, 1, 1), IsWorkday = false },
            new() { Date = new DateOnly(year, 1, 2), IsWorkday = true },
            new() { Date = new DateOnly(year, 1, 3), IsWorkday = true },
            new() { Date = new DateOnly(year, 1, 4), IsWorkday = true },
            new() { Date = new DateOnly(year, 1, 5), IsWorkday = true },
            new() { Date = new DateOnly(year, 1, 6), IsWorkday = true },
            new() { Date = new DateOnly(year, 1, 7), IsWorkday = false },
        };
        
        var actual = DefaultCalendarProvider.DefaultCalendar(year).Take(7);

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void Should_provide_correct_last_week_of_year()
    {
        const int year = 2023;
        var expected = new List<CalendarDate>
        {
            new() { Date = new DateOnly(year, 12, 25), IsWorkday = true },
            new() { Date = new DateOnly(year, 12, 26), IsWorkday = true },
            new() { Date = new DateOnly(year, 12, 27), IsWorkday = true },
            new() { Date = new DateOnly(year, 12, 28), IsWorkday = true },
            new() { Date = new DateOnly(year, 12, 29), IsWorkday = true },
            new() { Date = new DateOnly(year, 12, 30), IsWorkday = false },
            new() { Date = new DateOnly(year, 12, 31), IsWorkday = false },
        };
        
        var actual = DefaultCalendarProvider.DefaultCalendar(year).TakeLast(7);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Should_provide_current_year()
    {
        var year = Constants.CurrentYear;
        var expected = new List<CalendarDate>();
        for (var date = new DateOnly(year, 1, 1); date <= new DateOnly(year, 12, 31); date = date.AddDays(1))
        {
            expected.Add(new CalendarDate()
            {
                Date = date,
                IsWorkday = date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday
            });
        }

        var actual = DefaultCalendarProvider.DefaultCalendar(year).ToList();

        actual.Should().BeEquivalentTo(expected);
    }
}