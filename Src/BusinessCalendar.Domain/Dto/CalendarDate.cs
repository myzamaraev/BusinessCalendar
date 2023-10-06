namespace BusinessCalendar.Domain.Dto;

/// <summary>
/// represents a calendar date as an immutable record struct
/// </summary>
/// <param name="Date">specific date of the year</param>
/// <param name="IsWorkday">workday or not</param>
public readonly record struct CalendarDate(DateOnly Date, bool IsWorkday);
