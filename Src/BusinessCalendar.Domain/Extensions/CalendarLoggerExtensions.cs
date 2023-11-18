using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BusinessCalendar.Domain.Extensions;

public static class CalendarLoggerExtensions
{
    public static void LogDefaultCalendarUsageWarning(this ILogger<CalendarManagementService> logger, CalendarId calendarId)
    {
        logger.LogWarning("No calendar found for {CalendarId}. Default calendar will be provided", calendarId);
    }
    
    public static void LogCalendarUpdated(this ILogger<CalendarManagementService> logger, CompactCalendar calendar)
    {
        var serializedCalendar = SerializeCalendar(calendar, logger);
        
        logger.LogInformation("Calendar updated: {CalendarId}. New document: {CompactCalendarObject}", 
            calendar?.Id, 
            serializedCalendar);
    }

    private static string SerializeCalendar(CompactCalendar calendar, ILogger<CalendarManagementService> logger)
    {
        try
        {
            return JsonConvert.SerializeObject(calendar);
        }
        catch (Exception e)
        {
            logger.LogError(e,"Can't serialize CompactCalendar to JSON: {CalendarId}", calendar.Id);
            return string.Empty;
        }
    }
}