using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Mappers;
using Microsoft.Extensions.DependencyInjection;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Validators;
using FluentValidation;

namespace BusinessCalendar.Domain.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBusinessCalendarDomain(this IServiceCollection services)
        {
            services.AddSingleton<ICalendarIdentifierService, CalendarIdentifierService>();
            services.AddSingleton<ICalendarManagementService, CalendarManagementService>();
            services.AddSingleton<IValidator<CalendarId>, CalendarIdValidator>();
            services.AddSingleton<IValidator<CompactCalendar>, CompactCalendarValidator>();
            services.AddSingleton<IValidator<SaveCalendarRequest>, SaveCalendarRequestValidator>();
            services.AddSingleton<ICalendarMapper, CalendarMapper>();
        }
    }
}