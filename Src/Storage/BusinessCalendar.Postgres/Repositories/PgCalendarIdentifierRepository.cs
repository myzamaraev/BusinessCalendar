using BusinessCalendar.Domain.Dto;
using BusinessCalendar.Domain.Storage;
using BusinessCalendar.Postgres.Options;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace BusinessCalendar.Postgres.Repositories;

public class PgCalendarIdentifierRepository : ICalendarIdentifierStorageService
{
    private readonly NpgsqlDataSource _dataSource;

    public PgCalendarIdentifierRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public async Task InsertAsync(CalendarIdentifier calendarIdentifier, CancellationToken cancellationToken = default)
    {
        const string query = """INSERT INTO "CalendarIdentifier" ("Id", "Type", "Key") VALUES (@Id, @Type, @Key)""";
        
        var cmd = new CommandDefinition(
            query, 
            new
            {
                Id = calendarIdentifier.Id, 
                Type = calendarIdentifier.Type.ToString(), 
                Key = calendarIdentifier.Key
            }, 
            cancellationToken: cancellationToken);

        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(cmd);
    }

    public async Task<List<CalendarIdentifier>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        const string query = """
            SELECT * FROM "CalendarIdentifier" 
            ORDER BY "Type", "Key"
            LIMIT @PageSize OFFSET @Page
            """;
        
        var cmd = new CommandDefinition(
            query, 
            new { Page = page, PageSize = pageSize }, 
            cancellationToken: cancellationToken);
        
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync<CalendarIdentifier>(cmd);
        return result.ToList();
    }

    public async Task<CalendarIdentifier?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        const string query = """SELECT * FROM "CalendarIdentifier" WHERE "Id" = @Id""";
        var cmd = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);
        
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<CalendarIdentifier>(cmd);
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        const string query = """DELETE FROM "CalendarIdentifier" WHERE "Id" = @Id""";
        var cmd = new CommandDefinition(query, new { Id = id }, cancellationToken: cancellationToken);
        await using var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await connection.ExecuteAsync(cmd);
    }
}