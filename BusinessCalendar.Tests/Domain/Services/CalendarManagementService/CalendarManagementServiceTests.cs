using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.Domain.Mappers;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Validators;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagementService;

[TestFixture]
public partial class CalendarManagementServiceTests
{
    private Mock<ICalendarStorageService> _calendarStorageServiceMock;
    private Mock<CompactCalendarValidator> _compactCalendarValidatorMock;
    private Mock<SaveCalendarRequestValidator> _saveCalendarRequestValidatorMock;
    private Mock<ICalendarMapper> _calendarMapper;
    
    [SetUp]
    public void SetUp()
    {
        _calendarStorageServiceMock = new Mock<ICalendarStorageService>();
        _compactCalendarValidatorMock = new Mock<CompactCalendarValidator>();
        _saveCalendarRequestValidatorMock = new Mock<SaveCalendarRequestValidator>();
        _calendarMapper = new Mock<ICalendarMapper>();
    }
    
    

    private BusinessCalendar.Domain.Services.CalendarManagementService CreateCalendarManagementService()
    {
        return new BusinessCalendar.Domain.Services.CalendarManagementService(
                _calendarStorageServiceMock.Object,
                _compactCalendarValidatorMock.Object,
                _saveCalendarRequestValidatorMock.Object,
                _calendarMapper.Object
            );
}
}