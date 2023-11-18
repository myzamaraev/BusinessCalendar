using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Mappers;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Validators;
using FluentValidation;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

[TestFixture]
public partial class CalendarManagementServiceTests
{
    private Mock<ICalendarStorageService> _calendarStorageServiceMock;
    private Mock<CompactCalendarValidator> _compactCalendarValidatorMock;
    private Mock<SaveCalendarRequestValidator> _saveCalendarRequestValidatorMock;
    private Mock<CalendarIdValidator> _calendarIdValidatorMock;
    private Mock<ICalendarMapper> _calendarMapper;
    
    private readonly CalendarId DefaultCalendarId = new () { Year = Constants.CurrentYear };
   
    [SetUp]
    public void SetUp()
    {
        var calendarIdentifierServiceMock = new Mock<ICalendarIdentifierService>();
        _calendarIdValidatorMock = new Mock<CalendarIdValidator>(calendarIdentifierServiceMock.Object);
        
        _calendarStorageServiceMock = new Mock<ICalendarStorageService>();
        _compactCalendarValidatorMock = new Mock<CompactCalendarValidator>(_calendarIdValidatorMock.Object);
        _saveCalendarRequestValidatorMock = new Mock<SaveCalendarRequestValidator>(_calendarIdValidatorMock.Object);
        _calendarMapper = new Mock<ICalendarMapper>();
    }


    private CalendarManagementService CreateCalendarManagementService(
        IValidator<SaveCalendarRequest>? saveCalendarRequestValidator = null,
        IValidator<CompactCalendar>? compactCalendarValidator = null,
        IValidator<CalendarId>? calendarIdValidator = null)
    {
        return new CalendarManagementService(
            _calendarStorageServiceMock.Object,
            compactCalendarValidator ?? _compactCalendarValidatorMock.Object,
            saveCalendarRequestValidator ??_saveCalendarRequestValidatorMock.Object,
            calendarIdValidator ?? _calendarIdValidatorMock.Object,
            _calendarMapper.Object,
            new NullLogger<CalendarManagementService>()
        );
    }
}