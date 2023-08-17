using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Extensions;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

public partial class CalendarManagementServiceTests
{
    [Test]
    public async Task Should_SaveCalendarAsync_call_request_validator_once()
    {
        var request = new SaveCalendarRequest();

        await CreateCalendarManagementService().SaveCalendarAsync(request);
        
        _saveCalendarRequestValidatorMock.Verify(x => 
            x.ValidateAsync(
                It.Is<ValidationContext<SaveCalendarRequest>>(context => context.InstanceToValidate == request && context.ThrowOnFailures == true), 
                It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task Should_SaveCalendarAsync_call_CalendarMapper_once()
    {
        var request = new SaveCalendarRequest();

        await CreateCalendarManagementService().SaveCalendarAsync(request);

        _calendarMapper.Verify(x => 
            x.MapToCompact(It.Is<SaveCalendarRequest>(r => r == request)), Times.Once);
    }
    
    [Test]
    public async Task Should_SaveCalendarAsync_call_CompactCalendarValidator_once()
    {
        var request = new SaveCalendarRequest();

        var compactCalendar = new Calendar(CalendarType.State, "Test", DateTime.Today.Year).ToCompact();
        
        _calendarMapper.Setup(x => x.MapToCompact(It.IsAny<SaveCalendarRequest>()))
            .Returns(compactCalendar);

        await CreateCalendarManagementService().SaveCalendarAsync(request);
        
        _compactCalendarValidatorMock.Verify(x => 
            x.ValidateAsync(
                It.Is<ValidationContext<CompactCalendar>>(context => context.InstanceToValidate == compactCalendar && context.ThrowOnFailures == true), 
                It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Test]
    public async Task Should_SaveCalendarAsync_call_storage_once()
    {
        var request = new SaveCalendarRequest();
        var compactCalendar = new Calendar(new CalendarId { Year = Constants.CurrentYear }).ToCompact();
        
        _calendarMapper.Setup(x => x.MapToCompact(It.IsAny<SaveCalendarRequest>()))
            .Returns(compactCalendar);

        await CreateCalendarManagementService().SaveCalendarAsync(request);

        _calendarStorageServiceMock.Verify(x => 
            x.Upsert(It.Is<CompactCalendar>(c => c == compactCalendar)), Times.Once);
    }
}