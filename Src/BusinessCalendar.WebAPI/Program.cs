using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using BusinessCalendar.WebAPI.Extensions;
using BusinessCalendar.WebAPI.Options;
using BusinessCalendar.WebAPI.Swagger;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;
    options.MapClientException(StatusCodes.Status400BadRequest);
    options.MapFluentValidationException(StatusCodes.Status400BadRequest); 
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

builder.Services.AddOpenIdConnectAuth(builder.Configuration);
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddProblemDetailsConventions(); // Adds MVC conventions to work better with the ProblemDetails middleware.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = nameof(String).ToLower(), Format = "date" });

    if (!builder.Environment.IsDevelopment())
    {
        c.DocumentFilter<PublicApiDocumentFilter>(); //show only public API in spec
    }
});

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();
app.UseProblemDetails();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions() { MinimumSameSitePolicy = SameSiteMode.Lax }); //the only way to get it working with oidc redirects


var actionEndpointBuilder = app.MapControllers();
var authSettings = app.Services.GetRequiredService<IOptions<AuthOptions>>().Value;
if (authSettings is { UseOpenIdConnectAuth: false })
{
    actionEndpointBuilder.AllowAnonymous(); //Bypassing Auth with AllowAnonymousAttribute according to https://stackoverflow.com/a/62193352
}

//Swagger for public API endpoints is available in production by design.
//in this case BFF endpoints are filtered by PublicApiDocumentFilter
app.UseSwagger();
app.UseSwaggerUI();

app.MapHealthcheckEndpoints();
app.MapMetrics(); //prometheus /metrics endpoint
app.UseHttpMetrics(); //default asp.net core metrics
app.UseSpa(spa =>
{
}); //Handles all requests by returning the default page (wwwroot) for the Single Page Application (SPA).

app.Run();

if (app.Environment.IsDevelopment())
{
    IdentityModelEventSource.ShowPII = true; //meaningful personal information (claims) for identity debug purposes
}

public partial class Program { } //life hack to create WebApplicationFactory for integration tests