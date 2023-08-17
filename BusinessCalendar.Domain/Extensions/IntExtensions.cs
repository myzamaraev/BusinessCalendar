namespace BusinessCalendar.Domain.Extensions;

public static class IntExtensions
{
    /// <summary>
    /// Checks if the value in a range of valid years
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool IsValidYear(this int value)
    {
        return value >= DateTime.MinValue.Year && value <= DateTime.MaxValue.Year;
    }

    /// <summary>
    /// Checks the integer value is a valid year
    /// </summary>
    /// <param name="year"></param>
    /// <exception cref="ArgumentOutOfRangeException">The value is out of range between DateTime.MinValue and DateTime.MaxValue</exception>
    public static void CheckYearValidity(this int year)
    {
        if (!year.IsValidYear()) throw new ArgumentOutOfRangeException(nameof(year), "The year value is out of range between DateTime.MinValue and DateTime.MaxValue");
    }
}