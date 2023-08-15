using System.Text.Json.Serialization;
using BusinessCalendar.Domain.Exceptions;
using Microsoft.OpenApi.Models;
using BusinessCalendar.MongoDb.Extensions;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.MongoDb;
using BusinessCalendar.WebAPI.Extensions;
using Hellang.Middleware.ProblemDetails;
using Hellang.Middleware.ProblemDetails.Mvc;


MongoClassMapper.Register();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => false;//builder.Environment.IsDevelopment();
    options.MapToStatusCode<ClientException>(StatusCodes.Status400BadRequest);
    options.MapFluentValidationException(StatusCodes.Status400BadRequest); 
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddProblemDetailsConventions(); // Adds MVC conventions to work better with the ProblemDetails middleware.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.MapType<DateOnly>(() => new OpenApiSchema { Type = typeof(string).Name.ToLower(), Format = "date"});
});
builder.Services.AddMongoDbStorage(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddBusinessCalendarDomain();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseProblemDetails();
app.MapControllers();

app.Run();
