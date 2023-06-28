using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;

namespace BusinessCalendar.Domain.Storage
{
    public interface ICalendarIdentifierStorageService
    {
        public Task InsertAsync(CalendarIdentifier calendarIdentifier);

        public Task<List<CalendarIdentifier>> GetAllAsync(int page, int pageSize);
    }
}