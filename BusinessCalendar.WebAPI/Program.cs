using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using BusinessCalendar.MongoDb.Extensions;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.MongoDb;
using BusinessCalendar.WebAPI.Extensions;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;//builder.Environment.IsDevelopment();
    options.MapClientException(StatusCodes.Status400BadRequest);
    options.MapFluentValidationException(StatusCodes.Status400BadRequest); 
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddProblemDetailsConventions(); // Adds MVC conventions to work better with the ProblemDetails middleware.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = typeof(string).Name.ToLower(), Format = "date"});
});

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();
app.UseProblemDetails();
app.MapControllers();
app.UseHealthcheckEndpoints();
app.UseSpa(spa => { }); //Handles all requests by returning the default page (wwwroot) for the Single Page Application (SPA).

app.Run();
