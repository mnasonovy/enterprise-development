using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Analytics;
using Library.Application.Services;
using Library.Infrastructure.MongoEf.Database;
using Library.Infrastructure.MongoEf.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB connection
var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb")
    ?? "mongodb://mongodb:27017";

Console.WriteLine($"[INFO] MongoDB connection string: {mongoConnectionString}");

// Register MongoDbContext
builder.Services.AddDbContext<MongoDbContext>(options =>
    options.UseMongoDB(mongoConnectionString, "library"));

// Register all repositories (EF Core versions)
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookTypeRepository>();
builder.Services.AddScoped<PublisherRepository>();
builder.Services.AddScoped<ReaderRepository>();
builder.Services.AddScoped<IssueRepository>();

// Register all services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookTypeService, BookTypeService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
