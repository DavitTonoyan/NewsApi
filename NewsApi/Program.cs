using Microsoft.EntityFrameworkCore;
using NewsApi.DataTransferObjects;
using NewsApi.Models;
using NewsApi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


var logger = new LoggerConfiguration()
.ReadFrom.Configuration(builder.Configuration)
.Enrich.FromLogContext()
.CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


builder.Services.AddDbContext<NewsContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IService<Category, CategoryDto>, CategoryService>();
builder.Services.AddScoped<IService<News, NewsDto>, NewsService>();
builder.Services.AddScoped<INewsQueries, NewsService>();


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    try
    {
        await next(context);
    }
    catch (ArgumentException ex)
    {
        context.Response.StatusCode = 404;
        logger.Error(ex, "NotFound");
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        logger.Error(ex, "ServerInternalError");

    }
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
