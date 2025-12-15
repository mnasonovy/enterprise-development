using Library.Application.Services;
using Library.Infrastructure.MongoDb.Database;
using Library.Infrastructure.MongoDb.Repositories;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.Readers;
using Library.Application.Contracts.Issues;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB Configuration
var mongoSettings = builder.Configuration.GetSection("MongoDbSettings");
var connectionString = mongoSettings.GetValue<string>("ConnectionString")
    ?? throw new InvalidOperationException("MongoDB ConnectionString is not configured");
var databaseName = mongoSettings.GetValue<string>("DatabaseName")
    ?? throw new InvalidOperationException("MongoDB DatabaseName is not configured");

// Register MongoDB context as singleton
builder.Services.AddSingleton(new MongoDbContext(connectionString, databaseName));

// Register repositories as scoped
builder.Services.AddScoped<BookMongoRepository>();
builder.Services.AddScoped<AuthorMongoRepository>();
builder.Services.AddScoped<ReaderMongoRepository>();
builder.Services.AddScoped<IssueMongoRepository>();

// Register application services as scoped
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IIssueService, IssueService>();

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