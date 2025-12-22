using Library.Domain.RepositoryInterfaces;
using Library.Infrastructure.MongoEf.Database;
using Library.Infrastructure.MongoEf.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.MongoEf.Extensions;

/// <summary>
/// Extension методы для регистрации инфраструктурных сервисов (MongoDB, репозитории).
/// Предотвращает захламление Program.cs и обеспечивает чистоту кода.
/// Использует паттерн ServiceCollectionExtension для DI конфигурации.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует MongoDbContext в DI контейнере.
    /// Инициализирует подключение к MongoDB с использованием Entity Framework Core.
    /// </summary>
    /// <param name="services">IServiceCollection для регистрации сервисов</param>
    /// <param name="connectionString">MongoDB connection string (например: "mongodb://localhost:27017")</param>
    /// <param name="databaseName">Имя базы данных MongoDB (например: "library")</param>
    /// <returns>IServiceCollection для цепочки вызовов (Method Chaining)</returns>
    /// <exception cref="ArgumentNullException">Если connectionString или databaseName равны null или пусто</exception>
    /// <example>
    /// <code>
    /// builder.Services.AddMongoDbContext(
    ///     "mongodb://localhost:27017",
    ///     "library"
    /// );
    /// </code>
    /// </example>
    public static IServiceCollection AddMongoDbContext(
        this IServiceCollection services,
        string connectionString,
        string databaseName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString),
                "MongoDB connection string cannot be null or empty");

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentNullException(nameof(databaseName),
                "MongoDB database name cannot be null or empty");

        services.AddDbContext<MongoDbContext>(options =>
            options.UseMongoDB(connectionString, databaseName)
        );

        return services;
    }

    /// <summary>
    /// Регистрирует все репозитории в DI контейнере как Scoped сервисы.
    /// Каждый репозиторий отвечает за CRUD операции для одной сущности.
    /// 
    /// Регистрируемые репозитории:
    /// - IBookRepository → BookRepository (управление книгами)
    /// - IAuthorRepository → AuthorRepository (управление авторами)
    /// - IReaderRepository → ReaderRepository (управление читателями)
    /// - IIssueRepository → IssueRepository (управление выданными книгами)
    /// - IPublisherRepository → PublisherRepository (управление издателями)
    /// - IBookTypeRepository → BookTypeRepository (управление типами книг)
    /// - IAnalyticsRepository → AnalyticsRepository (аналитические запросы)
    /// </summary>
    /// <param name="services">IServiceCollection для регистрации сервисов</param>
    /// <returns>IServiceCollection для цепочки вызовов (Method Chaining)</returns>
    /// <remarks>
    /// Lifetime: Scoped - создаётся новый экземпляр на каждый HTTP запрос.
    /// Это необходимо для изоляции данных между запросами и правильной работы DbContext.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services
    ///     .AddMongoDbContext(connectionString, databaseName)
    ///     .AddRepositories();  // Регистрирует все репозитории
    /// </code>
    /// </example>
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services
            .AddScoped<IBookRepository, BookRepository>()
            .AddScoped<IAuthorRepository, AuthorRepository>()
            .AddScoped<IReaderRepository, ReaderRepository>()
            .AddScoped<IIssueRepository, IssueRepository>()
            .AddScoped<IPublisherRepository, PublisherRepository>()
            .AddScoped<IBookTypeRepository, BookTypeRepository>()
            .AddScoped<IAnalyticsRepository, AnalyticsRepository>();

        return services;
    }
}
