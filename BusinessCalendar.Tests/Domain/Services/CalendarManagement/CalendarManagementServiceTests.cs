using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Mappers;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Domain.Validators;
using Moq;

namespace BusinessCalendar.Tests.Domain.Services.CalendarManagement;

[TestFixture]
public partial class CalendarManagementServiceTests
{
    private Mock<ICalendarStorageService> _calendarStorageServiceMock;
    private Mock<CompactCalendarValidator> _compactCalendarValidatorMock;
    private Mock<SaveCalendarRequestValidator> _saveCalendarRequestValidatorMock;
    private Mock<SaveCompactCalendarRequestValidator> _saveCompactCalendarRequestValidatorMock;
    private Mock<ICalendarMapper> _calendarMapper;
    
    private readonly CalendarId DefaultCalendarId = new () { Year = Constants.CurrentYear };

    [SetUp]
    public void SetUp()
    {
        _calendarStorageServiceMock = new Mock<ICalendarStorageService>();
        _compactCalendarValidatorMock = new Mock<CompactCalendarValidator>();
        _saveCalendarRequestValidatorMock = new Mock<SaveCalendarRequestValidator>();
        _saveCompactCalendarRequestValidatorMock = new Mock<SaveCompactCalendarRequestValidator>();
        _calendarMapper = new Mock<ICalendarMapper>();
    }


    private CalendarManagementService CreateCalendarManagementService()
    {
        return new CalendarManagementService(
            _calendarStorageServiceMock.Object,
            _compactCalendarValidatorMock.Object,
            _saveCalendarRequestValidatorMock.Object,
            _saveCompactCalendarRequestValidatorMock.Object,
            _calendarMapper.Object
        );
    }
}