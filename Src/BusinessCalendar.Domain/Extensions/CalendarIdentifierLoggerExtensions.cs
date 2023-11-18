using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Services;
using Microsoft.Extensions.Logging;

namespace BusinessCalendar.Domain.Extensions;

public static class CalendarIdentifierLoggerExtensions
{
    public static void LogCreated(this ILogger<CalendarIdentifierService> logger, CalendarIdentifier calendarIdentifier)
    {
        logger.LogInformation("Calendar identifier created: {CalendarIdentifierId}", calendarIdentifier.Id);
    }

    public static void LogDeleted(this ILogger<CalendarIdentifierService> logger, CalendarIdentifier calendarIdentifier)
    {
        logger.LogInformation("Calendar identifier deleted: {CalendarIdentifierId}", calendarIdentifier.Id);
    }

    public static void LogCantDeleteNotExisting(this ILogger<CalendarIdentifierService> logger, string id)
    {
        logger.LogError("Calendar identifier not found for deletion: {CalendarIdentifierId}", id);
    }
    
    public static void LogPageSizeLimited(this ILogger<CalendarIdentifierService> logger, int newPageSize)
    {
        logger.LogInformation("Page size was limited to {CalendarIdentifier_PageSizeLimit}", newPageSize);
    }
    
    public static void LogCalendarsDeleted(this ILogger<CalendarIdentifierService> logger, CalendarIdentifier calendarIdentifier, long deletedCount)
    {
        logger.LogInformation(
            "{DeletedCount} calendars with type {CalendarType} and key {CalendarKey} have been deleted during {CalendarIdentifierId} identifier deletion process", 
            deletedCount,
            calendarIdentifier.Type,
            calendarIdentifier.Key,
            calendarIdentifier.Id);
    }
    
    
}