using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Validators;

namespace BusinessCalendar.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessCalendarDomain(this IServiceCollection services)
        {
            services.AddSingleton<ICalendarManagementService, CalendarManagementService>();
            services.AddSingleton<CompactCalendarValidator>();
        }
    }
}