using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using BusinessCalendar.MongoDb.Extensions;
using BusinessCalendar.Domain.Extensions;
using BusinessCalendar.MongoDb;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

MongoClassMapper.Register();

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
app.MapControllers();

app.Run();
