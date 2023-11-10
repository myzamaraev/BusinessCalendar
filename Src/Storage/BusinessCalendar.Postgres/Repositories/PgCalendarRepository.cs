using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Enums;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Postgres.Options;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;

namespace BusinessCalendar.Postgres.Repositories;

public class PgCalendarRepository : ICalendarStorageService
{
    private readonly NpgsqlDataSource _dataSource;

    public PgCalendarRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public async Task UpsertAsync(CompactCalendar compactCalendar, CancellationToken cancellationToken = default)
    {
        const string query = """
            INSERT INTO "Calendar" ("Type", "Key", "Year", "Document")
            VALUES(@Type, @Key, @Year, @Document) 
            ON CONFLICT ON CONSTRAINT "PK_Calendar"
            DO UPDATE SET "Document" = @Document;
            """;
        
        var cmd = new CommandDefinition(
            query, 
            new
            {
                Type = compactCalendar.Id.Type.ToString(),
                Key = compactCalendar.Id.Key,
                Year = compactCalendar.Id.Year,
                Document = JsonConvert.SerializeObject(compactCalendar)
            }, 
            cancellationToken: cancellationToken);
        
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(cmd);
    }

    public  async Task<CompactCalendar?> FindOneAsync(CalendarId id, CancellationToken cancellationToken = default)
    {
        const string query = """
            SELECT "Document" AS Document FROM "Calendar" 
            WHERE "Type" = @Type AND "Key" = @Key AND "Year" = @Year
            LIMIT 1
            """;
        
        var cmd = new CommandDefinition(
            query, 
            new { Type = id.Type.ToString(), Key = id.Key, Year = id.Year }, 
            cancellationToken: cancellationToken);
        
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        var jsonDocument = await connection.ExecuteScalarAsync<string>(cmd);
        return DeserializeCalendar(jsonDocument);
    }

    public async Task DeleteManyAsync(CalendarType type, string key, CancellationToken cancellationToken = default)
    {
        const string query = """DELETE FROM "Calendar" WHERE "Type" = @Type AND "Key" = @Key""";
        
        var cmd = new CommandDefinition(
            query, 
            new
            {
                Type = type.ToString(),
                Key = key,
            }, 
            cancellationToken: cancellationToken);
        
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(cmd);
    }

    /// <summary>
    /// Custom deserialization, as neither System.Text.Json nor Json.Net can't restore domain object with immutable fields properly
    /// </summary>
    /// <param name="jsonDocument">serialized document as json string</param>
    /// <returns cref="CompactCalendar"></returns>
    /// <exception cref="JsonException"></exception>
    private CompactCalendar? DeserializeCalendar(string? jsonDocument)
    {
        if (string.IsNullOrWhiteSpace(jsonDocument))
        {
            return null;
        }
        
        try
        {
            var document = JsonConvert.DeserializeObject<dynamic>(jsonDocument);
            if (document == null)
            {
                return null;
            }
            
            CalendarType type = document.Id.Type;
            string key = document.Id.Key;
            int year = document.Id.Year;
            var holidays = document.Holidays.ToObject<List<DateOnly>>();
            var extraWorkDays = document.ExtraWorkDays.ToObject<List<DateOnly>>();
            return new CompactCalendar(new CalendarId(type, key, year), holidays, extraWorkDays);
        }
        catch
        {
            throw new JsonException("Document model doesn't match with CompactCalendar model");
        }
    }
}