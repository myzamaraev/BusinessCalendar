using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Services;

namespace BusinessCalendar.Tests;

internal class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateTestCalendar()
    {
        // var calendar = new Calendar(CalendarType.State, "US", 2023);
        //
        // //doesn't work anymore, because of struct value type. It's forbidden to change value in enumerator
        // calendar.Dates.ForEach(x => {
        //     if (x.Date >= new DateOnly(2023,01,01) && x.Date < new DateOnly(2023,01,9)) 
        //         x.IsWorkday = false;
        // });
        //
        // //doesn't work either
        // // foreach (var date in calendar.Dates.Where(x =>
        // //     x.Date >= new DateOnly(2023,01,01) && x.Date < new DateOnly(2023,01,9)))
        // // {
        // //     date.IsWorkday = false;
        // // }
        //     
        //
        // await _calendarManagementService.SaveCalendarAsync(calendar);
     
        Assert.Pass();
    }
}