using BusinessCalendar.Client.Dto;
using BusinessCalendar.Client.Providers;
using BusinessCalendar.Client.Providers.Dependencies;

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

        Assert.That(actual, Is.EqualTo(expected));
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

        Assert.That(actual, Is.EqualTo(expected));
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
            .ReturnsAsync(new CalendarModel { Year = 2023 });

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

    [TestCase(false)]
    [TestCase(true)]
    public async Task Should_IsWorkday_use_GetCalendar_when_FullCalendarCache_enabled(bool expected)
    {
        var today = DateTime.Today;

        var calendarMock = new Mock<CalendarModel>();
        calendarMock.Setup(calendar => calendar.IsWorkday(It.IsAny<DateTime>()))
            .Returns(expected);

        //Arrange
        _cacheProviderMock.Setup(cache =>
                cache.GetOrCreateAsync(It.IsAny<string>(), It.IsAny<Func<Task<CalendarModel>>>()))
            .ReturnsAsync(calendarMock.Object);

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

        calendarMock.Verify(calendar =>
                calendar.IsWorkday(It.Is<DateTime>(date => date.Equals(today))),
            Times.Once());

        Assert.That(actual, Is.EqualTo(expected));
    }
}