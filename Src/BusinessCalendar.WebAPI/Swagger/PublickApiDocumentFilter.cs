using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using MongoDB.Driver.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BusinessCalendar.WebAPI.Swagger;

public class PublicApiDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        //Remove all Paths except public api endpoints
        var otherRoutes = swaggerDoc.Paths
            .Where(x => !x.Key.ToLower().Contains("api"))
            .ToList();
        
        otherRoutes.ForEach(x =>
        {
            swaggerDoc.Paths.Remove(x.Key); 
        });

        //Remove all schemas not referenced by remaining Paths
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