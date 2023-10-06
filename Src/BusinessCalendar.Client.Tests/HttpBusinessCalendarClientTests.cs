using BusinessCalendar.Client.Dto;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.MongoDb.Options;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;

namespace BusinessCalendar.Client.Tests;

public class HttpBusinessCalendarClientTests
{
    private MongoDbContainer _mongoDbContainer;
    private WebApplicationFactory<Program> _webApplicationFactory;
    private HttpClient _httpClient;
    private ICalendarIdentifierStorageService _calendarIdentifierStorageService;
    private ICalendarStorageService _calendarStorageService;
    

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        _mongoDbContainer = new MongoDbBuilder().WithImage("mongo:6.0.4").Build();
        await _mongoDbContainer.StartAsync().ConfigureAwait(false);
        
        _webApplicationFactory = new WebApplicationFactory<Program>();
        _webApplicationFactory = _webApplicationFactory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.Configure<MongoDbOptions>(options =>
                {
                    options.ConnectionUri = _mongoDbContainer.GetConnectionString();;
                });
            });
        });

        _httpClient = _webApplicationFactory.CreateClient();
        _calendarIdentifierStorageService = _webApplicationFactory.Services.GetRequiredService<ICalendarIdentifierStorageService>();
        _calendarStorageService = _webApplicationFactory.Services.GetRequiredService<ICalendarStorageService>();
    }
    

    [SetUp]
    public async Task SetUp()
    {
        await _calendarIdentifierStorageService.InsertAsync(new CalendarIdentifier(CalendarType.Custom, "Test"));
        await _calendarStorageService.UpsertAsync(new CompactCalendar(
            new CalendarId() { Year = 2023, Type = CalendarType.Custom, Key = "Test" },
            new List<DateOnly>
            {
                new DateOnly(2023, 01, 02)
            },
            new List<DateOnly>()
            {
                new DateOnly(2023, 01, 01)
            }));
    }

    [TearDown]
    public async Task TearDown()
    {
        await _calendarStorageService.DeleteManyAsync(CalendarType.Custom, "Test");
        await _calendarIdentifierStorageService.DeleteAsync("Custom_Test");
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        await _mongoDbContainer.StopAsync().ConfigureAwait(false);
    }

    [Test]
    public async Task Should_GetCalendarAsync_use_api_and_get_calendar()
    {
        var businessCalendarClient = new HttpBusinessCalendarClient(_httpClient);
        var expected = new CalendarModel
        {
            Type = "Custom",
            Key = "Test",
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

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task Should_GetCalendarAsync_use_default_calendar_when_not_found_in_db()
    {
        var persistentCalendar = await _calendarStorageService.FindOneAsync(new CalendarId(CalendarType.Custom, "Test", 2025));
        Assert.That(persistentCalendar, Is.Null); //Ensure no such calendar in DB before testing
        
        var businessCalendarClient = new HttpBusinessCalendarClient(_httpClient);
        var expected = new CalendarModel
        {
            Type = "Custom",
            Key = "Test",
            Year = 2025
            //expecting no extra workdays and holidays
        };

        var actual = await businessCalendarClient.GetCalendarAsync("Custom_Test", 2025);

        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task Should_GetDateAsync_use_api_and_get_calendar_date()
    {
        var businessCalendarClient = new HttpBusinessCalendarClient(_httpClient);
        var expected = new CalendarDateModel()
        {
            Type = "Custom",
            Key = "Test",
            Date = new DateTime(2023, 01, 01),
            IsWorkday = true
        };

        var actual = await businessCalendarClient.GetDateAsync("Custom_Test", new DateTime(2023, 01, 01));

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public async Task Should_GetDateAsync_use_default_calendar_when_not_found_in_db()
    {
        var persistentCalendar = await _calendarStorageService.FindOneAsync(new CalendarId(CalendarType.Custom, "Test", 2025));
        Assert.That(persistentCalendar, Is.Null); //Ensure no such calendar in DB before testing
        
        var businessCalendarClient = new HttpBusinessCalendarClient(_httpClient);
        var expected = new CalendarDateModel()
        {
            Type = "Custom",
            Key = "Test",
            Date = new DateTime(2025, 01, 01),
            IsWorkday = true
        };

        var actual = await businessCalendarClient.GetDateAsync("Custom_Test", new DateTime(2025, 01, 01));

        actual.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void ShouldGetCalendarAsync_throw_when_wrong_identifier()
    {
        new HttpBusinessCalendarClient(_httpClient)
            .Invoking(x => x.GetCalendarAsync("InvalidIdentifier", 2023))
            .Should()
            .ThrowExactlyAsync<DocumentNotFoundClientException>()
            .WithMessage("No such calendar identifier found: InvalidIdentifier");
    }

    [Test]
    public void ShouldGetDateAsync_throw_when_wrong_identifier()
    {
        new HttpBusinessCalendarClient(_httpClient)
            .Invoking(x => x.GetDateAsync("InvalidIdentifier", new DateTime(2023, 01, 01)))
            .Should()
            .ThrowExactlyAsync<DocumentNotFoundClientException>()
            .WithMessage("No such calendar identifier found: InvalidIdentifier");
    }
}