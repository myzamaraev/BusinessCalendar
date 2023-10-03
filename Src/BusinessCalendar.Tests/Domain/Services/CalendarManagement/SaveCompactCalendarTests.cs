using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Services;
using FluentAssertions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

public partial class CalendarManagementServiceTests
{
    
    [Test]
    public async Task Should_SaveCompactCalendar_call_CalendarMapper_once()
    {
        var request = new SaveCompactCalendarRequest();

        await CreateCalendarManagementService().SaveCompactCalendarAsync(request);

        _calendarMapper.Verify(x => 
            x.MapToCompact(It.Is<SaveCompactCalendarRequest>(r => r == request)), Times.Once);
    }
    
    [Test]
    public async Task Should_SaveCompactCalendar_call_CompactCalendarValidator_once()
    {
        var request = new SaveCompactCalendarRequest();

        var compactCalendar = new Calendar(CalendarType.State, "Test", DateTime.Today.Year).ToCompact();
        
        _calendarMapper.Setup(x => x.MapToCompact(It.IsAny<SaveCompactCalendarRequest>()))
            .Returns(compactCalendar);

        await CreateCalendarManagementService().SaveCompactCalendarAsync(request);
        
        _compactCalendarValidatorMock.Verify(x => 
            x.ValidateAsync(
                It.Is<ValidationContext<CompactCalendar>>(context => context.InstanceToValidate == compactCalendar && context.ThrowOnFailures == true), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }
    
    [Test]
    public async Task Should_SaveCompactCalendar_throw_when_CompactCalendarValidator_failed()
    {
        var fakeValidator = TestValidator.Faulty<CompactCalendar>();
        var saveCompactCalendarRequest = new SaveCompactCalendarRequest()
            {};
        
        _calendarMapper.Setup(x => x.MapToCompact(It.IsAny<SaveCompactCalendarRequest>()))
            .Returns(new CompactCalendar(new CalendarId()));

        var actual = await CreateCalendarManagementService(compactCalendarValidator: fakeValidator)
            .Invoking(x => x.SaveCompactCalendarAsync(saveCompactCalendarRequest))
            .Should()
            .ThrowExactlyAsync<ValidationException>();

        actual.Subject.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().HaveCount(1);
        actual.Subject.Single().Errors.Should().Contain(failure => failure.ErrorMessage == "Failure");
    }

    [Test]
    public async Task Should_SaveCompactCalendar_call_storage_once()
    {
        var request = new SaveCompactCalendarRequest();
        var compactCalendar = new Calendar(CalendarType.State, "Test", DateTime.Today.Year).ToCompact();
        
        _calendarMapper.Setup(x => x.MapToCompact(It.IsAny<SaveCompactCalendarRequest>()))
            .Returns(compactCalendar);

        await CreateCalendarManagementService().SaveCompactCalendarAsync(request);

        _calendarStorageServiceMock.Verify(x => 
            x.UpsertAsync(
                It.Is<CompactCalendar>(c => c == compactCalendar), 
                It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}