using BusinessCalendar.Client.Dto;
using BusinessCalendar.Client.Providers;
using BusinessCalendar.Client.Providers.Dependencies;
using FluentAssertions;

namespace BusinessCalendar.Client.Tests;

public class WorkdayProviderTests
{
    private Mock<IBusinessCalendarClient> _businessCalendarClientMock;
    private Mock<ICacheProvider> _cacheProviderMock;

    [SetUp]
    public void SetUp()
    {
        _businessCalendarClientMock = new Mock<IBusinessCalendarClient>();
        _cacheProviderMock = new Mock<ICacheProvider>();
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task Should_IsWorkday_use_GetDate(bool expected)
    {
        //Arrange
        _businessCalendarClientMock
            .Setup(client => client.GetDateAsync(It.IsAny<string>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new CalendarDateModel { IsWorkday = expected });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object);

        //Act
        var actual = await sut.IsWorkday("State_Test", DateTime.Today);

        //Assert
        _businessCalendarClientMock.Verify(client =>
                client.GetDateAsync(
                    It.Is<string>(identifier => identifier == "State_Test"),
                    It.Is<DateTime>(date => date == DateTime.Today)),
            Times.Once);

        actual.Should().Be(expected);
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task Should_IsWorkday_use_GetDate_cached_value(bool expected)
    {
        //Arrange
        var today = DateTime.Today;

        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarDateModel>>>()))
            .ReturnsAsync(new CalendarDateModel { IsWorkday = expected })
            ;

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object,
            options => options.UseCache(_cacheProviderMock.Object));

        //Act
        var actual = await sut.IsWorkday("State_Test", today);

        //Assert
        _businessCalendarClientMock.Verify(client =>
                client.GetDateAsync(
                    It.Is<string>(identifier => identifier == "State_Test"),
                    It.Is<DateTime>(date => date.Equals(today))),
            Times.Never);

        _cacheProviderMock.Verify(cache =>
                cache.GetOrCreateAsync(
                    It.Is<string>(cacheKey =>
                        cacheKey == $"WorkdayProvider_GetDateAsync_State_Test_{today:yyyy-MM-dd}"),
                    It.IsAny<Func<Task<CalendarDateModel>>>()),
            Times.Once());

        actual.Should().Be(expected);
    }

    [Test]
    public async Task Should_IsWorkday_provide_GetDate_delegate_for_CacheProvider()
    {
        //Arrange
        var today = DateTime.Today;

        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarDateModel>>>()))
            .Callback(async (string identifier, Func<Task<CalendarDateModel>> createItemFunc) =>
            {
                await createItemFunc.Invoke();
            })
            .ReturnsAsync(new CalendarDateModel());

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object,
            options => options.UseCache(_cacheProviderMock.Object));

        //Act
        var actual = await sut.IsWorkday("State_Test", today);

        //Assert
        _businessCalendarClientMock.Verify(client =>
                client.GetDateAsync(
                    It.Is<string>(identifier => identifier == "State_Test"),
                    It.Is<DateTime>(date => date.Equals(today))),
            Times.Once);
    }

    [Test]
    public async Task Should_IsWorkday_provide_GetCalendar_delegate_for_CacheProvider_when_FullCalendarCache_enabled()
    {
        //Arrange
        var today = DateTime.Today;

        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarModel>>>()))
            .Callback(async (string identifier, Func<Task<CalendarModel>> createItemFunc) =>
            {
                await createItemFunc.Invoke();
            })
            .ReturnsAsync(new CalendarModel { Year = today.Year });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object,
            options => options.UseFullCalendarCache(_cacheProviderMock.Object));

        //Act
        var actual = await sut.IsWorkday("State_Test", today);

        //Assert
        _businessCalendarClientMock.Verify(client =>
                client.GetCalendarAsync(
                    It.Is<string>(identifier => identifier == "State_Test"),
                    It.Is<int>(year => year.Equals(today.Year))),
            Times.Once);
    }

    [Test]
    public async Task Should_IsWorkday_not_use_GetDate_when_FullCalendarCache_enabled()
    {
        //Arrange
        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarDateModel>>>()))
            .ReturnsAsync(new CalendarDateModel());

        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarModel>>>()))
            .ReturnsAsync(new CalendarModel { Year = DateTime.Today.Year });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object,
            options => options.UseFullCalendarCache(_cacheProviderMock.Object));

        //Act
        await sut.IsWorkday("State_Test", DateTime.Today);

        //Assert
        _cacheProviderMock.Verify(cache => cache.GetOrCreateAsync(
                It.IsAny<string>(),
                It.IsAny<Func<Task<CalendarDateModel>>>()),
            Times.Never);
        _businessCalendarClientMock.Verify(client =>
                client.GetDateAsync(It.IsAny<string>(), It.IsAny<DateTime>()),
            Times.Never);
    }
    
    [Test]
    public async Task Should_IsWorkday_use_GetCalendar_when_FullCalendarCache_enabled()
    {
        //Arrange
        var today =  DateTime.Today;
        var calendarModel = new CalendarModel()
        {
            Year = today.Year
        };

        var expected = calendarModel.IsWorkday(today);

        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarModel>>>()))
            .ReturnsAsync(calendarModel);

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object,
            options => options.UseFullCalendarCache(_cacheProviderMock.Object));

        //Act
        var actual = await sut.IsWorkday("State_Test", today);

        //Assert
        _cacheProviderMock.Verify(cache =>
                cache.GetOrCreateAsync(
                    It.Is<string>(cacheKey =>
                        cacheKey == $"WorkdayProvider_GetCalendarAsync_State_Test_{today:yyyy}"),
                    It.IsAny<Func<Task<CalendarModel>>>()),
            Times.Once);

        actual.Should().Be(expected);
    }

    [TestCase("2024-03-07", 1, false, "2024-03-08")] // Thu +1 calendar day → Fri (workday)
    [TestCase("2024-03-07", 2, false, "2024-03-11")] // Thu +2 calendar days → Sat → Mon
    [TestCase("2024-03-07", 3, true,  "2024-03-12")] // Thu +3 workdays: Fri=1, Mon=2, Tue=3 → Tue
    [TestCase("2024-03-08", 1, true,  "2024-03-11")] // Fri +1 workday: Sat=off, Sun=off, Mon=1 → Mon
    public async Task Should_GetNextWorkday_return_correct_date(string baseDateStr, int offset, bool workdaysOnly, string expectedStr)
    {
        //Arrange
        var baseDate = DateTime.Parse(baseDateStr);
        var expected = DateTime.Parse(expectedStr);

        _businessCalendarClientMock
            .Setup(client => client.GetCalendarAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((string _, int year) => new CalendarModel { Year = year });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object);

        //Act
        var result = await sut.GetNextWorkday("State_Test", baseDate, offset, workdaysOnly);

        //Assert
        result.Should().Be(expected);
    }

    [TestCase(false)] // Thu +1 calendar day → Fri (holiday) → skips to Mon
    [TestCase(true)]  // Thu +1 workday → Fri (holiday) skipped → Mon
    public async Task Should_GetNextWorkday_skip_holiday(bool workdaysOnly)
    {
        //Arrange
        var thursday = new DateTime(2024, 3, 7);
        var friday = new DateTime(2024, 3, 8);
        var expectedMonday = new DateTime(2024, 3, 11);

        _businessCalendarClientMock
            .Setup(client => client.GetCalendarAsync("State_Test", 2024))
            .ReturnsAsync(new CalendarModel { Year = 2024, Holidays = new List<DateTime> { friday } });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object);

        //Act
        var result = await sut.GetNextWorkday("State_Test", thursday, 1, workdaysOnly);

        //Assert
        result.Should().Be(expectedMonday);
    }

    [TestCase(false)] // Dec 31 2024 +1 calendar day → Jan 1 2025; only 2025 calendar fetched
    [TestCase(true)]  // Dec 31 2024 +1 workday → Jan 1 2025; only 2025 calendar fetched
    public async Task Should_GetNextWorkday_fetch_next_year_calendar_on_year_rollover(bool workdaysOnly)
    {
        //Arrange
        var dec31 = new DateTime(2024, 12, 31);
        var expectedJan1 = new DateTime(2025, 1, 1);

        _businessCalendarClientMock
            .Setup(client => client.GetCalendarAsync("State_Test", 2025))
            .ReturnsAsync(new CalendarModel { Year = 2025 });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object);

        //Act
        var result = await sut.GetNextWorkday("State_Test", dec31, 1, workdaysOnly);

        //Assert
        result.Should().Be(expectedJan1);
        _businessCalendarClientMock.Verify(client => client.GetCalendarAsync("State_Test", 2025), Times.Once);
        _businessCalendarClientMock.Verify(client => client.GetCalendarAsync("State_Test", 2024), Times.Never);
    }

    [TestCase(false)] // Fri +1 calendar day → Sat (ExtraWorkDay)
    [TestCase(true)]  // Fri +1 workday → Sat (ExtraWorkDay counts as workday)
    public async Task Should_GetNextWorkday_return_extra_workday_on_weekend(bool workdaysOnly)
    {
        //Arrange
        var friday = new DateTime(2024, 3, 8);
        var saturday = new DateTime(2024, 3, 9);

        _businessCalendarClientMock
            .Setup(client => client.GetCalendarAsync("State_Test", 2024))
            .ReturnsAsync(new CalendarModel { Year = 2024, ExtraWorkDays = new List<DateTime> { saturday } });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object);

        //Act
        var result = await sut.GetNextWorkday("State_Test", friday, 1, workdaysOnly);

        //Assert
        result.Should().Be(saturday);
    }

    [TestCase(false)] // all days are holidays → no workday found → throws
    [TestCase(true)]  // all days are holidays → workday count never reached → throws
    public async Task Should_GetNextWorkday_throw_when_no_workday_found_within_365_days(bool workdaysOnly)
    {
        //Arrange
        _businessCalendarClientMock
            .Setup(client => client.GetCalendarAsync(It.IsAny<string>(), It.IsAny<int>()))
            .ReturnsAsync((string _, int year) => new CalendarModel
            {
                Year = year,
                Holidays = Enumerable.Range(0, DateTime.IsLeapYear(year) ? 366 : 365)
                    .Select(i => new DateTime(year, 1, 1).AddDays(i))
                    .ToList()
            });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object);

        //Act
        var act = () => sut.GetNextWorkday("State_Test", new DateTime(2024, 1, 1), 1, workdaysOnly);

        //Assert
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task Should_GetNextWorkday_use_cache_when_enabled()
    {
        //Arrange
        var thursday = new DateTime(2024, 3, 7);

        _cacheProviderMock
            .Setup(cache => cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarModel>>>()))
            .ReturnsAsync(new CalendarModel { Year = 2024 });

        var sut = new WorkdayProvider(_businessCalendarClientMock.Object,
            options => options.UseCache(_cacheProviderMock.Object));

        //Act
        await sut.GetNextWorkday("State_Test", thursday, 1, false);

        //Assert
        _cacheProviderMock.Verify(cache => cache.GetOrCreateAsync(
                It.Is<string>(key => key == "WorkdayProvider_GetCalendarAsync_State_Test_2024"),
                It.IsAny<Func<Task<CalendarModel>>>()),
            Times.Once);
        _businessCalendarClientMock.Verify(client =>
                client.GetCalendarAsync(It.IsAny<string>(), It.IsAny<int>()),
            Times.Never);
    }
}