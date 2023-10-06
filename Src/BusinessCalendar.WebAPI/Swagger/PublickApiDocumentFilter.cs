using Microsoft.OpenApi.Models;
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
        var requiredSchemas = new List<string>();

        requiredSchemas.AddRange(
            swaggerDoc.Paths.SelectMany(x => x.Value.Operations.Values)
                .SelectMany(x => x.Responses.Values)
                .SelectMany(x => x.Content.Values)
                .Select(x => x.Schema.Reference.Id)
                .Distinct());

        requiredSchemas.AddRange(
            swaggerDoc.Paths.SelectMany(x => x.Value.Operations.Values)
                .SelectMany(x => x.Parameters)
                .Where(x => x.Schema is { Reference: not null })
                .Select(x => x.Schema.Reference.Id)
                .Distinct()
        );

        foreach (var schema in swaggerDoc.Components.Schemas)
        {
            if (!requiredSchemas.Contains(schema.Key))
            {
                swaggerDoc.Components.Schemas.Remove(schema.Key);
            }
        }
    }
}