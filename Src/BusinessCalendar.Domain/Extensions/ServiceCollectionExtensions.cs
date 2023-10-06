using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Dto.Requests;
using BusinessCalendar.Domain.Mappers;
using Microsoft.Extensions.DependencyInjection;
using BusinessCalendar.Domain.Services;
using BusinessCalendar.Domain.Validators;
using FluentValidation;

namespace BusinessCalendar.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds domain services to ServiceCollection
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBusinessCalendarDomain(this IServiceCollection services)
    {
        services.AddSingleton<ICalendarIdentifierService, CalendarIdentifierService>();
        services.AddSingleton<ICalendarManagementService, CalendarManagementService>();
        services.AddSingleton<IValidator<CalendarId>, CalendarIdValidator>();
        services.AddSingleton<IValidator<CompactCalendar>, CompactCalendarValidator>();
        services.AddSingleton<IValidator<SaveCalendarRequest>, SaveCalendarRequestValidator>();
        services.AddSingleton<IValidator<AddCalendarIdentifierRequest>, AddCalendarIdentifierRequestValidator>();
        services.AddSingleton<ICalendarMapper, CalendarMapper>();
        services.AddSingleton<IClientCalendarService, ClientCalendarService>();

        return services;
    }
}