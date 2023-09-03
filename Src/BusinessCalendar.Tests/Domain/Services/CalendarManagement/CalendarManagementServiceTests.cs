using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Mappers;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Validators;
using FluentValidation;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

[TestFixture]
public partial class CalendarManagementServiceTests
{
    private Mock<ICalendarStorageService> _calendarStorageServiceMock;
    private Mock<CompactCalendarValidator> _compactCalendarValidatorMock;
    private Mock<SaveCalendarRequestValidator> _saveCalendarRequestValidatorMock;
    private Mock<ICalendarMapper> _calendarMapper;
    
    private readonly CalendarId DefaultCalendarId = new () { Year = Constants.CurrentYear };

    [SetUp]
    public void SetUp()
    {
        var calendarIdValidatorMock = new Mock<CalendarIdValidator>();
        
        _calendarStorageServiceMock = new Mock<ICalendarStorageService>();
        _compactCalendarValidatorMock = new Mock<CompactCalendarValidator>(calendarIdValidatorMock.Object);
        _saveCalendarRequestValidatorMock = new Mock<SaveCalendarRequestValidator>(calendarIdValidatorMock.Object);
        _calendarMapper = new Mock<ICalendarMapper>();
    }


    private CalendarManagementService CreateCalendarManagementService(
        IValidator<SaveCalendarRequest>? saveCalendarRequestValidator = null,
        IValidator<CompactCalendar>? compactCalendarValidator = null)
    {
        return new CalendarManagementService(
            _calendarStorageServiceMock.Object,
            compactCalendarValidator ?? _compactCalendarValidatorMock.Object,
            saveCalendarRequestValidator ??_saveCalendarRequestValidatorMock.Object,
            _calendarMapper.Object
        );
    }
}