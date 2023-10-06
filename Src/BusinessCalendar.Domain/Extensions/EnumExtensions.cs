using System.ComponentModel;

namespace BusinessCalendar.Domain.Extensions;

public static class EnumExtensions
{
    /// <summary>
    /// Gets the value from Description attribute
    /// </summary>
    /// <param name="value"></param>
    /// <returns>string value from description attribute</returns>
    public static string GetDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        if (fieldInfo == null)
        {
            return value.ToString();
        }
        
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return attributes is { Length: > 0 } ? attributes[0].Description : value.ToString();
    }
}