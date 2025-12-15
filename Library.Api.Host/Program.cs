using Library.Application.Services;
using Library.Infrastructure.MongoDb.Database;
using Library.Infrastructure.MongoDb.Repositories;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Analytics;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB Configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb")
    ?? "mongodb://localhost:27017";
var databaseName = "LibraryDb";

// Register MongoDB client and context
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddSingleton(new MongoDbContext(mongoConnectionString, databaseName));

// Register repositories as scoped
builder.Services.AddScoped<AuthorMongoRepository>();
builder.Services.AddScoped<BookMongoRepository>();
builder.Services.AddScoped<ReaderMongoRepository>();
builder.Services.AddScoped<IssueMongoRepository>();
builder.Services.AddScoped<PublisherMongoRepository>();
builder.Services.AddScoped<BookTypeMongoRepository>();

// Register application services as scoped
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookTypeService, BookTypeService>();
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