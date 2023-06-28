using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;

namespace BusinessCalendar.Domain.Dto
{
    public class CalendarIdentifier
    {
        public string Id { get; set; }
        public CalendarType Type { get; set; }
        public string Key { get; set; }

        public CalendarIdentifier(CalendarType type, string key)
        {
            Type = type;
            Key = key;
            Id = $"{type}_{key}";
        }
    }
}