using System;
using System.Threading.Tasks;
using BusinessCalendar.Contracts.ApiContracts;

namespace BusinessCalendar.Client
{
    public interface IBusinessCalendarClient
    {
        Task<GetCalendarResponse> GetCalendarAsync(string identifier, int year);

        Task<GetCalendarDateResponse> GetDateAsync(string identifier, DateTime date);
    }
}
