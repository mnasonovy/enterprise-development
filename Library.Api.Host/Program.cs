using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;

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

// Register MongoDB EF Core context
builder.Services.AddDbContext<MongoDbContext>(options =>
    options.UseMongoDB(mongoConnectionString, "library"));

// Register repositories
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<BookTypeRepository>();
builder.Services.AddScoped<IssueRepository>();
builder.Services.AddScoped<PublisherRepository>();
builder.Services.AddScoped<ReaderRepository>();

// Register application services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IBookTypeService, BookTypeService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

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