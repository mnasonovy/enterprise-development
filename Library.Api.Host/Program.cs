using Library.Api.Host;
using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Application.Services;
using Library.Infrastructure.MongoEf;
using Library.Infrastructure.MongoEf.Database;
using Library.Infrastructure.MongoEf.Repositories;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// ========================================
// КОНФИГУРАЦИЯ ЛОГИРОВАНИЯ
// ========================================
/// <summary>
/// Настраивает логирование для приложения.
/// Очищает стандартные провайдеры и добавляет логирование в консоль с уровнем Information.
/// </summary>
builder.Services.AddLogging(configure =>
{
    configure.ClearProviders();
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Information);
});

// ========================================
// КОНФИГУРАЦИЯ БАЗЫ ДАННЫХ MONGODB
// ========================================
/// <summary>
/// Подключает MongoDB Entity Framework Core к приложению.
/// Используется строка подключения из конфигурации или локальное подключение по умолчанию.
/// retryWrites=false и w=1 используются из-за ограничений MongoDB в Docker контейнере.
/// </summary>
var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb")
    ?? "mongodb://localhost:27017/?retryWrites=false&w=1";

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    options.UseMongoDB(mongoConnectionString, "LibraryDb");
});

// ========================================
// КОНФИГУРАЦИЯ AUTOMAPPER
// ========================================
/// <summary>
/// Регистрирует AutoMapper для преобразования объектов между моделями и DTO.
/// Подключает MappingProfile и автоматически обнаруживает профили в сборках.
/// </summary>
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
    config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

// ========================================
// РЕГИСТРАЦИЯ РЕПОЗИТОРИЕВ
// ========================================
/// <summary>
/// Регистрирует все репозитории как Scoped сервисы.
/// Каждый запрос получает новый экземпляр репозитория.
/// </summary>
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<ReaderRepository>();
builder.Services.AddScoped<IssueRepository>();
builder.Services.AddScoped<PublisherRepository>();
builder.Services.AddScoped<BookTypeRepository>();

// ========================================
// РЕГИСТРАЦИЯ СЕРВИСОВ ПРИЛОЖЕНИЯ
// ========================================
/// <summary>
/// Регистрирует все сервисы приложения как Scoped зависимости.
/// Сервисы реализуют интерфейсы контрактов для работы контроллеров.
/// </summary>
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookTypeService, BookTypeService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

// ========================================
// РЕГИСТРАЦИЯ КОНТРОЛЛЕРОВ
// ========================================
/// <summary>
/// Добавляет маршрутизацию контроллеров для REST API.
/// </summary>
builder.Services.AddControllers();

// ========================================
// КОНФИГУРАЦИЯ SWAGGER / OPENAPI
// ========================================
/// <summary>
/// Настраивает Swagger для интерактивной документации API.
/// Подключает XML комментарии для полной документации методов контроллеров.
/// Отображает метаинформацию об API и контакты поддержки.
/// </summary>
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1",
        Description = "REST API для управления библиотекой. Поддерживает полный набор CRUD операций для книг, авторов, читателей, выпусков, издателей и типов книг.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Library Support",
            Url = new Uri("https://localhost:7000")
        }
    });

    // Подключает XML комментарии из сборки для документации в Swagger
    var xmlFile = Path.Combine(AppContext.BaseDirectory, "Library.Api.Host.xml");
    if (File.Exists(xmlFile))
        c.IncludeXmlComments(xmlFile);
});

builder.Services.AddEndpointsApiExplorer();

// ========================================
// ПОСТРОЕНИЕ И ЗАПУСК ПРИЛОЖЕНИЯ
// ========================================
/// <summary>
/// Строит приложение и настраивает middleware pipeline.
/// </summary>
var app = builder.Build();

// Включает Swagger только в режиме разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        c.RoutePrefix = string.Empty; // Swagger будет доступен по адресу /
    });
}

// Middleware pipeline для обработки запросов
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

/// <summary>
/// Запускает приложение и начинает прослушивание входящих запросов.
/// </summary>
app.Run();
