using Library.Domain.RepositoryInterfaces;
using Library.Infrastructure.MongoEf.Database;
using Library.Infrastructure.MongoEf.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.MongoEf.Extensions;

/// <summary>
/// Методы расширения для регистрации MongoDB и репозиториев в DI контейнере.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует MongoDbContext в DI контейнере.
    /// </summary>
    public static IServiceCollection AddMongoDbContext(
        this IServiceCollection services,
        string connectionString,
        string databaseName)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentNullException(nameof(connectionString));

        if (string.IsNullOrWhiteSpace(databaseName))
            throw new ArgumentNullException(nameof(databaseName));

        services.AddDbContext<MongoDbContext>(options =>
            options.UseMongoDB(connectionString, databaseName)
        );

        return services;
    }

    /// <summary>
    /// Регистрирует все репозитории как Scoped сервисы.
    /// </summary>
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
