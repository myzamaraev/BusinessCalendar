using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BusinessCalendar.WebAPI.Swagger;

/// <summary>
/// Filters out everything but public api endpoints
/// </summary>
public class PublicApiDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var notPublicApiPaths = swaggerDoc.Paths
            .Where(x => !x.Key.ToLower().Contains("api"))
            .ToList();

        notPublicApiPaths.ForEach(x =>
        {
            swaggerDoc.Paths.Remove(x.Key);
        });

        RemoveAbandonedSchemas(swaggerDoc);
    }

    private static void RemoveAbandonedSchemas(OpenApiDocument swaggerDoc)
    {
        if (swaggerDoc.Components?.Schemas is not { Count: > 0 }) return;

        var operations = swaggerDoc.Paths.Values
            .OfType<OpenApiPathItem>()
            .SelectMany(p => (p.Operations ?? []).Values);

        var responseSchemaIds = operations
            .SelectMany(o => o.Responses?.Select(r => r.Value).OfType<OpenApiResponse>() ?? Enumerable.Empty<OpenApiResponse>())
            .SelectMany(r => r.Content?.Values ?? Enumerable.Empty<OpenApiMediaType>())
            .Select(m => m.Schema as OpenApiSchemaReference);

        var parameterSchemaIds = operations
            .SelectMany(o => o.Parameters?.OfType<OpenApiParameter>() ?? Enumerable.Empty<OpenApiParameter>())
            .Select(p => p.Schema as OpenApiSchemaReference);

        var referencedIds = responseSchemaIds
            .Concat(parameterSchemaIds)
            .Select(r => r?.Reference.Id)
            .OfType<string>()
            .ToHashSet();

        foreach (var key in swaggerDoc.Components.Schemas.Keys.Except(referencedIds).ToList())
            swaggerDoc.Components.Schemas.Remove(key);
    }
}
