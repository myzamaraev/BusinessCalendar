using System.ComponentModel;
using System.Reflection;

namespace BusinessCalendar.Domain.Extensions;

public static class EnumExtensions
{
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