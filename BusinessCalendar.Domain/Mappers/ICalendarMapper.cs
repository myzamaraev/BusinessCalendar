using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;

namespace BusinessCalendar.Domain.Mappers;

public interface ICalendarMapper
{
    public Calendar Map(SaveCalendarRequest saveCalendarRequest);
    public CompactCalendar MapToCompact(SaveCalendarRequest saveCalendarRequest);

    public CompactCalendar MapToCompact(Calendar calendar);

    public CompactCalendar MapToCompact(SaveCompactCalendarRequest request);
}