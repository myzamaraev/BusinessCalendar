using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Exceptions;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Validators;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services;

[TestFixture]
public class CalendarIdentifierServiceTests
{
    private Mock<ICalendarIdentifierStorageService> _calendarIdentifierStorageServiceMock;
    private Mock<ICalendarStorageService> _calendarStorageServiceMock;
    private Mock<AddCalendarIdentifierRequestValidator> _addCalendarIdentifierRequestValidator;

    [SetUp]
    public void SetUp()
    {
        _calendarIdentifierStorageServiceMock = new Mock<ICalendarIdentifierStorageService>();
        _calendarStorageServiceMock = new Mock<ICalendarStorageService>();
        _addCalendarIdentifierRequestValidator = new Mock<AddCalendarIdentifierRequestValidator>();
    }

    [Test]
    public async Task Should_AddCalendarIdentifierAsync_call_AddCalendarIdentifierRequestValidator()
    {
        var addCalendarIdentifierRequest = new AddCalendarIdentifierRequest()
        {
            Key = "Test"
        };

        await CreateCalendarIdentifierService().AddCalendarIdentifierAsync(addCalendarIdentifierRequest);

        _addCalendarIdentifierRequestValidator.Verify(x =>
                x.ValidateAsync(
                    It.Is<ValidationContext<AddCalendarIdentifierRequest>>(context =>
                        context.InstanceToValidate == addCalendarIdentifierRequest
                        && context.ThrowOnFailures == true),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public async Task Should_AddCalendarIdentifierAsync_throw_when_AddCalendarIdentifierRequestValidator_failed()
    {
        var request = new AddCalendarIdentifierRequest();
        var fakeValidator = TestValidator.Faulty<AddCalendarIdentifierRequest>();
        var actual = await CreateCalendarIdentifierService(fakeValidator)
            .Invoking(x => x.AddCalendarIdentifierAsync(request))
            .Should()
            .ThrowExactlyAsync<ValidationException>();

        actual.Subject.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().Contain(failure => failure.ErrorMessage == "Failure");
    }

    [Test]
    public async Task Should_AddCalendarIdentifierAsync_call_storage()
    {
        var addCalendarIdentifierRequest = new AddCalendarIdentifierRequest()
        {
            Type = CalendarType.State,
            Key = "Test"
        };

        await CreateCalendarIdentifierService().AddCalendarIdentifierAsync(addCalendarIdentifierRequest);

        _calendarIdentifierStorageServiceMock.Verify(x => x.InsertAsync(
            It.Is<CalendarIdentifier>(calendarIdentifier =>
                calendarIdentifier.Type == CalendarType.State
                && calendarIdentifier.Key == "Test"
                && calendarIdentifier.Id == "State_Test"), 
            It.IsAny<CancellationToken>()));
    }

    [Test]
    public async Task Should_DeleteCalendarIdentifierAsync_call_Find_once()
    {
        const string id = "TestId";

        _calendarIdentifierStorageServiceMock.Setup(x => x.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CalendarIdentifier(CalendarType.State, "testKey"));

        await CreateCalendarIdentifierService().DeleteCalendarIdentifierAsync(id);

        _calendarIdentifierStorageServiceMock.Verify(x =>
                x.GetAsync(It.Is<string>(actualId => actualId == id), It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test]
    public async Task Should_DeleteCalendarIdentifierAsync_throw_when_no_CalendarIdentifier_found()
    {
        const string id = "TestId";

        await CreateCalendarIdentifierService()
            .Invoking(x => x.DeleteCalendarIdentifierAsync(id))
            .Should()
            .ThrowExactlyAsync<DocumentNotFoundClientException>()
            .WithMessage($"Calendar identifier {id} not found");
    }
    
    [Test]
    public async Task Should_DeleteCalendarIdentifierAsync_call_storage_DeleteAsync_once()
    {
        _calendarIdentifierStorageServiceMock.Setup(x => x.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CalendarIdentifier(CalendarType.State, "Test_FromIdentifier"));

        await CreateCalendarIdentifierService().DeleteCalendarIdentifierAsync("FakeTestId");

        _calendarIdentifierStorageServiceMock.Verify(x =>
                x.DeleteAsync(It.Is<string>(actualId => actualId == "State_Test_FromIdentifier"),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [Test]
    public async Task Should_DeleteCalendarIdentifierAsync_call_CalendarStorageService_DeleteMany_once()
    {
        _calendarIdentifierStorageServiceMock.Setup(x => x.GetAsync(
                It.IsAny<string>(), 
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new CalendarIdentifier(CalendarType.State, "Test_FromIdentifier"));

        await CreateCalendarIdentifierService().DeleteCalendarIdentifierAsync("FakeTestId");

        _calendarStorageServiceMock.Verify(x =>
                x.DeleteManyAsync(
                    It.Is<CalendarType>(type => type == CalendarType.State), 
                    It.Is<string>(key => key == "Test_FromIdentifier"),
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }
    
    [TestCase(0)]
    [TestCase(1)]
    [TestCase(25)]
    public async Task Should_GetCalendarIdentifiersAsync_not_throw_when_page_positive(int page)
    {
        var actual = await CreateCalendarIdentifierService()
            .Invoking(x => x.GetCalendarIdentifiersAsync(0, 10))
            .Should()
            .NotThrowAsync();
    }
    
    [TestCase(-1)]
    [TestCase(-25)]
    public async Task Should_GetCalendarIdentifiersAsync_throw_when_page_negative(int page)
    {
        var actual = await CreateCalendarIdentifierService()
            .Invoking(x => x.GetCalendarIdentifiersAsync(page, 10))
            .Should()
            .ThrowExactlyAsync<ArgumentClientException>()
            .WithMessage($"Argument page can't have value {page}");
    }

    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-25)]
    public async Task Should_GetCalendarIdentifiersAsync_throw_when_pageSIze_less_than_one(int pageSize)
    {
        var actual = await CreateCalendarIdentifierService()
            .Invoking(x => x.GetCalendarIdentifiersAsync(0, pageSize))
            .Should()
            .ThrowExactlyAsync<ArgumentClientException>()
            .WithMessage($"Argument pageSize can't have value {pageSize}");
    }
    
    [TestCase(1)]
    [TestCase(25)]
    public async Task Should_GetCalendarIdentifiersAsync_not_throw_when_page_one_or_more(int pageSize)
    {
        var actual = await CreateCalendarIdentifierService()
            .Invoking(x => x.GetCalendarIdentifiersAsync(0, pageSize))
            .Should()
            .NotThrowAsync();
    }
    
    [TestCase(0, 1)]
    [TestCase(1, 50)]
    [TestCase(2, 100)]
    public async Task Should_GetCalendarIdentifiersAsync_call_storage_GetAll_once(int page, int pageSize)
    {
        await CreateCalendarIdentifierService().GetCalendarIdentifiersAsync(page, pageSize);
        
        _calendarIdentifierStorageServiceMock.Verify(x => 
            x.GetAllAsync(
                It.Is<int>(actualPage => actualPage == page), 
                It.Is<int>(actualPageSize => actualPageSize == pageSize),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [TestCase(101)]
    [TestCase(200)]
    public async Task Should_GetCalendarIdentifiersAsync_limit_pageSize_when_more_than_100(int pageSize)
    {
        await CreateCalendarIdentifierService().GetCalendarIdentifiersAsync(0, pageSize);
        
        _calendarIdentifierStorageServiceMock.Verify(x => 
            x.GetAllAsync(
                It.IsAny<int>(), 
                It.Is<int>(actualPageSize => actualPageSize == 100),
                It.IsAny<CancellationToken>()));
    }
    

    private CalendarIdentifierService CreateCalendarIdentifierService(
        IValidator<AddCalendarIdentifierRequest>? addCalendarIdentifierRequestValidator = null)
    {
        return new CalendarIdentifierService(
            _calendarIdentifierStorageServiceMock.Object,
            _calendarStorageServiceMock.Object,
            addCalendarIdentifierRequestValidator ?? _addCalendarIdentifierRequestValidator.Object);
    }
}