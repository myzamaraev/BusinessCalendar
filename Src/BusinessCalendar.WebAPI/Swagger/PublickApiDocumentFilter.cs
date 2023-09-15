using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BusinessCalendar.WebAPI.Swagger;

public class PublicApiDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var publicApiRoutes = swaggerDoc.Paths
            .Where(x => !x.Key.ToLower().Contains("api"))
            .ToList();
        publicApiRoutes.ForEach(x => { swaggerDoc.Paths.Remove(x.Key); });
    }
}