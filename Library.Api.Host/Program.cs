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

/// <summary>
/// Конфигурация логирования приложения.
/// Настраивает консольный вывод с уровнем Information для отслеживания операций.
/// </summary>
builder.Services.AddLogging(configure =>
{
    configure.ClearProviders();
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Information);
});

/// <summary>
/// Подключение MongoDB с использованием Entity Framework Core.
/// Подключение берется из конфигурации или использует localhost.
/// Database: LibraryDb, retryWrites=false для Docker совместимости.
/// </summary>
var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb")
    ?? "mongodb://localhost:27017/?retryWrites=false&w=1";

builder.Services.AddDbContext<MongoDbContext>(options =>
{
    options.UseMongoDB(mongoConnectionString, "LibraryDb");
});

/// <summary>
/// Регистрация AutoMapper для преобразования Domain моделей в DTO.
/// MappingProfile содержит все двусторонние маппинги (ReverseMap).
/// </summary>
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
    config.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

/// <summary>
/// Регистрация репозиториев как Scoped сервисы.
/// Каждый репозиторий отвечает за CRUD операции одной сущности.
/// Lifecycle: создается новый экземпляр на каждый HTTP запрос.
/// - BookRepository: операции с книгами
/// - AuthorRepository: операции с авторами
/// - ReaderRepository: операции с читателями
/// - IssueRepository: операции с выданными книгами
/// - PublisherRepository: операции с издателями
/// - BookTypeRepository: операции с типами книг
/// </summary>
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<AuthorRepository>();
builder.Services.AddScoped<ReaderRepository>();
builder.Services.AddScoped<IssueRepository>();
builder.Services.AddScoped<PublisherRepository>();
builder.Services.AddScoped<BookTypeRepository>();

/// <summary>
/// Регистрация сервисов приложения как Scoped зависимости.
/// Сервисы содержат бизнес-логику и используют репозитории для доступа к БД.
/// - IBookService: управление книгами (CRUD + поиск)
/// - IAuthorService: управление авторами
/// - IReaderService: управление читателями и их данными
/// - IIssueService: управление выданными книгами и сроками возврата
/// - IPublisherService: управление издателями
/// - IBookTypeService: управление типами книг
/// - IAnalyticsService: аналитические отчеты и статистика
/// </summary>
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IReaderService, ReaderService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddScoped<IBookTypeService, BookTypeService>();
builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();

/// <summary>
/// Регистрация контроллеров и маршрутизации REST API.
/// Автоматически обнаруживает все контроллеры, помеченные [ApiController].
/// </summary>
builder.Services.AddControllers();

/// <summary>
/// Конфигурация Swagger для интерактивной документации API.
/// Генерирует OpenAPI схему и предоставляет UI для тестирования методов.
/// XML комментарии из документации подключаются автоматически.
/// </summary>
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1",
        Description = "REST API для управления библиотекой\n\n" +
                      "Функциональность:\n" +
                      "- 📚 Управление книгами, авторами, читателями\n" +
                      "- 📖 Отслеживание выданных книг и сроков возврата\n" +
                      "- 🏢 Издатели и типы книг\n" +
                      "- 📊 Аналитические отчеты и статистика",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Library Support",
            Url = new Uri("https://localhost:7000")
        }
    });

    var xmlFile = Path.Combine(AppContext.BaseDirectory, "Library.Api.Host.xml");
    if (File.Exists(xmlFile))
        c.IncludeXmlComments(xmlFile);
});

builder.Services.AddEndpointsApiExplorer();

/// <summary>
/// Построение приложения и настройка middleware pipeline.
/// Middleware обрабатывает HTTP запросы в следующем порядке:
/// 1. HttpsRedirection - перенаправление на HTTPS
/// 2. Authorization - проверка прав доступа
/// 3. MapControllers - маршрутизация к контроллерам
/// 4. Swagger (только разработка) - интерактивная документация
/// </summary>
var app = builder.Build();

/// <summary>
/// Включение Swagger только в режиме разработки.
/// В production Swagger отключается по соображениям безопасности.
/// Доступен по адресу https://localhost:7000/
/// </summary>
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        c.RoutePrefix = string.Empty;
    });
}

/// <summary>
/// Включение middleware для обработки запросов.
/// - UseHttpsRedirection: перенаправляет HTTP на HTTPS
/// - UseAuthorization: проверка авторизации
/// - MapControllers: маршрутизация к контроллерам
/// </summary>
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

/// <summary>
/// Запуск приложения и начало прослушивания входящих HTTP(S) запросов.
/// Порты:
/// - HTTPS: https://localhost:7000
/// - HTTP: http://localhost:5000
/// - Swagger UI: https://localhost:7000 (режим разработки)
/// </summary>
app.Run();
