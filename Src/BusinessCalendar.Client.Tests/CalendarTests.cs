using System.Globalization;
using BusinessCalendar.Client.Dto;

namespace BusinessCalendar.Client.Tests;

public class CalendarTests
{
    [TestCaseSource(nameof(DayOffTestCaseSource))]
    public void Should_IsWorkday_return_true_when_ExtraWorkDay(DateTime dayOff)
    {
        var calendar = new CalendarModel
        {
            Year = 2023,
            ExtraWorkDays = { dayOff }
        };

        var actual = calendar.IsWorkday(dayOff);
        
        Assert.That(actual, Is.True);
    }
    
    [TestCaseSource(nameof(DayOffTestCaseSource))]
    public void Should_IsWorkday_return_false_when_not_ExtraWorkDay(DateTime dayOff)
    {
        var calendar = new CalendarModel
        {
            Year = 2023,
        };

        var actual = calendar.IsWorkday(dayOff);
        
        Assert.That(actual, Is.False);
    }
    
    [TestCaseSource(nameof(WorkDayTestCaseSource))]
    public void Should_IsWorkday_return_false_when_Holiday(DateTime workday)
    {
        var calendar = new CalendarModel
        {
            Year = 2023,
            Holidays = { workday }
        };

        var actual = calendar.IsWorkday(workday);
        
        Assert.That(actual, Is.False);
    }
    
    [TestCaseSource(nameof(WorkDayTestCaseSource))]
    public void Should_IsWorkday_return_true_when_not_Holiday(DateTime workday)
    {
        var calendar = new CalendarModel
        {
            Year = 2023
        };

        var actual = calendar.IsWorkday(workday);
        
        Assert.That(actual, Is.True);
    }

    [TestCase("2022-12-31")]
    [TestCase("2024-01-01")]
    public void Should_IsWorkday_throw_when_wrong_year(string dateString)
    {
        var calendar = new CalendarModel
        {
            Year = 2023
        };

        var date = DateTime.ParseExact(dateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => calendar.IsWorkday(date));
        Assert.That(exception?.Message, Is.EqualTo($"Date {dateString} is out of the year 2023 (Parameter 'date')"));
    }

    private static IEnumerable<DateTime> WorkDayTestCaseSource
    {
        get
        {
            yield return new DateTime(2023, 01, 02);
            yield return new DateTime(2023, 01, 03);
            yield return new DateTime(2023, 01, 04);
            yield return new DateTime(2023, 01, 05);
            yield return new DateTime(2023, 01, 06);
        }
    }
    
    private static IEnumerable<DateTime> DayOffTestCaseSource
    {
        get
        {
            yield return new DateTime(2023, 01, 01);
            yield return new DateTime(2023, 01, 07);
            yield return new DateTime(2023, 01, 08);
        }
    }
}