using Library.Application.Contracts.Authors;
using Library.Application.Services;
using Library.Infrastructure.MongoEf.Database;
using Library.Infrastructure.MongoEf.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB EF Core configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb");

if (string.IsNullOrEmpty(mongoConnectionString))
    throw new InvalidOperationException("MongoDB connection string is not configured");

Console.WriteLine($"[DEBUG] MongoDB connection string: {mongoConnectionString}");

// регистрируем контекст MongoEF
builder.Services.AddDbContext<MongoDbContext>(options =>
    options.UseMongoDB(mongoConnectionString, "library"));

// регистрируем авторский репозиторий и сервис
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
