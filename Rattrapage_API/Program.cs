using Microsoft.EntityFrameworkCore;
using Rattrapage_API.Services;
using NLog;
using NLog.Web;
using NLog.Extensions.Logging;
using Rattrapage_API.Middlewares;
using Rattrapage_API.Interface;
using Rattrapage_API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configure NLog
LogManager.LoadConfiguration("nlog.config");

// Add NLog to the logging pipeline
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace); // Set minimum logging level to Trace
builder.Logging.AddNLog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Register ArtistesService
builder.Services.AddScoped<IArtistesRepository, ArtistesRepository>();
builder.Services.AddScoped<IArtistesService, ArtistesService>();
builder.Services.AddScoped<ArtistesService>();
builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
