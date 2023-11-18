using System.Text.Json.Serialization;
using BusinessCalendar.WebAPI.Extensions;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ExtendConfiguration(builder.Environment.EnvironmentName);
builder.Host.UseSerilog((context, sp, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;
    options.MapClientException(StatusCodes.Status400BadRequest);
    options.MapFluentValidationException(StatusCodes.Status400BadRequest); 
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
    options.ShouldLogUnhandledException = (_, _, _) => true; //log everything (not only Status>=500 by default)
});

builder.Services.AddOpenIdConnectAuth(builder.Configuration, showPII: builder.Environment.IsDevelopment());
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddProblemDetailsConventions(); // Adds MVC conventions to work better with the ProblemDetails middleware.

builder.Services.AddOpenApiDocumentation(publicApiOnly: !builder.Environment.IsDevelopment());
builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();
app.ApplyDatabaseMigrations();
app.UseSerilogRequestLogging(options => options.EnrichRequestContext());
app.UseProblemDetails();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions() { MinimumSameSitePolicy = SameSiteMode.Lax }); //the only way to get it working with oidc redirects
app.MapEndpoints();
app.Run();

public partial class Program { } //life hack to create WebApplicationFactory for integration tests