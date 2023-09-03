using BusinessCalendar.Client.Dto;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using DateTime = System.DateTime;

namespace BusinessCalendar.Client.Tests;

public class HttpBusinessCalendarClientTests
{
    private WebApplicationFactory<Program> _webApplicationFactory;
    private Mock<ICalendarStorageService> _calendarStorageServiceMock;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _webApplicationFactory = new WebApplicationFactory<Program>();
    }

    [SetUp]
    public void SetUp()
    {
        _calendarStorageServiceMock = new Mock<ICalendarStorageService>();
        _calendarStorageServiceMock.Setup(x => x.FindOne(It.IsAny<CalendarId>()))
            .ReturnsAsync(new CompactCalendar(
                new CalendarId() { Year = 2023, Type = CalendarType.Custom, Key = "ResponseKey" },
                new List<DateOnly>
                {
                    new DateOnly(2023, 01, 02)
                },
                new List<DateOnly>()
                {
                    new DateOnly(2023, 01, 01)
                }));
        
        _webApplicationFactory = _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var storageDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICalendarStorageService));
                if (storageDescriptor != null)
                {
                    services.Remove(storageDescriptor);
                }
                
                services.AddSingleton<ICalendarStorageService, ICalendarStorageService>((sp) => _calendarStorageServiceMock.Object);
            });
        });
    }


    [Test]
    public async Task Should_GetCalendarAsync_use_api_and_get_calendar()
    {
        var httpClient = _webApplicationFactory.CreateClient();
        var businessCalendarClient = new HttpBusinessCalendarClient(httpClient);
        var expected = new CalendarModel
        {
            Type = "Custom",
            Key = "ResponseKey",
            Year = 2023,
            Holidays = new List<DateTime>
            {
                new DateTime(2023, 01, 02)
            },
            ExtraWorkDays = new List<DateTime>
            {
                new DateTime(2023, 01, 01)
            }
        };

        var actual = await businessCalendarClient.GetCalendarAsync("Custom_Test", 2023);


        _calendarStorageServiceMock.Verify(x => x.FindOne(It.Is<CalendarId>(id =>
                id.Type == CalendarType.Custom
                && id.Key == "Test"
                && id.Year == 2023)),
            Times.Once);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Should_GetDateAsync_use_api_and_get_calendar_date()
    {
        var httpClient = _webApplicationFactory.CreateClient();
        var businessCalendarClient = new HttpBusinessCalendarClient(httpClient);
        var expected = new CalendarDateModel()
        {
            Type = "Custom",
            Key = "ResponseKey",
            Date = new DateTime(2023, 01, 01),
            IsWorkday = true
        };

        var actual = await businessCalendarClient.GetDateAsync("Custom_Test", new DateTime(2023, 01, 01));

        _calendarStorageServiceMock.Verify(x => x.FindOne(It.Is<CalendarId>(id =>
                id.Type == CalendarType.Custom
                && id.Key == "Test"
                && id.Year == 2023)),
            Times.Once);

        actual.Should().BeEquivalentTo(expected);
    }
}