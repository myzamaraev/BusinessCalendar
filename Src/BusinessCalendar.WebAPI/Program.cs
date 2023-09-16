using System.Security.Authentication;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using BusinessCalendar.WebAPI.Extensions;
using BusinessCalendar.WebAPI.Options;
using BusinessCalendar.WebAPI.Swagger;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;//builder.Environment.IsDevelopment();
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
builder.Services.AddSwaggerGen(c => {
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = nameof(String).ToLower(), Format = "date"});

    if (!builder.Environment.IsDevelopment())
    {
        c.DocumentFilter<PublicApiDocumentFilter>(); //show only public API in spec
    }
});

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    IdentityModelEventSource.ShowPII = true; //meaningful personal information (claims) for identity debug purposes
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseProblemDetails();

var actionEndpointBuilder = app.MapControllers();
var authSettings = builder.Configuration.GetSection(AuthOptions.Section).Get<AuthOptions>();
if (authSettings is { UseOpenIdConnectAuth: false})
{
    actionEndpointBuilder.AllowAnonymous(); //Bypassing Auth with AllowAnonymousAttribute according to https://stackoverflow.com/a/62193352
}

app.UseHealthcheckEndpoints();
app.UseSpa(spa => { }); //Handles all requests by returning the default page (wwwroot) for the Single Page Application (SPA).

app.Run();


public partial class Program { } //life hack to create WebApplicationFactory for integration tests
