using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Storage
{
    public interface ICalendarStorageService
    {
        public Task Upsert(CompactCalendar compactCalendar);
        public Task<CompactCalendar> FindOne(CalendarType type, string key, int year);
        public Task DeleteMany(CalendarType type, string key);
    }
}