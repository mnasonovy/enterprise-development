using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Application.Extensions;

/// <summary>
/// Extension методы для регистрации Application Layer сервисов в DI контейнере.
/// Упаковывает регистрацию бизнес-логики сервисов и конфигурацию маппинга.
/// Использует паттерн ServiceCollectionExtension для чистоты Program.cs.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все Application сервисы в DI контейнере как Scoped сервисы.
    /// Каждый сервис содержит бизнес-логику и зависит от соответствующего репозитория.
    /// 
    /// Регистрируемые сервисы:
    /// - IBookService → BookService (логика управления книгами)
    /// - IAuthorService → AuthorService (логика управления авторами)
    /// - IReaderService → ReaderService (логика управления читателями)
    /// - IIssueService → IssueService (логика управления выданными книгами)
    /// - IPublisherService → PublisherService (логика управления издателями)
    /// - IBookTypeService → BookTypeService (логика управления типами книг)
    /// - IAnalyticsService → AnalyticsService (аналитика и статистика библиотеки)
    /// </summary>
    /// <param name="services">IServiceCollection для регистрации сервисов</param>
    /// <returns>IServiceCollection для цепочки вызовов (Method Chaining)</returns>
    /// <remarks>
    /// Lifetime: Scoped - создаётся новый экземпляр на каждый HTTP запрос.
    /// Это необходимо для изоляции бизнес-логики между запросами.
    /// 
    /// Архитектура слоёв:
    /// Controller → Service → Repository → DbContext
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services
    ///     .AddApplicationServices()    // Регистрирует все 7 сервисов
    ///     .AddAutoMapperConfiguration();
    /// </code>
    /// </example>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // ========================================
        // РЕГИСТРАЦИЯ APPLICATION СЕРВИСОВ
        // ========================================
        // Каждый сервис регистрируется как Scoped:
        // - Зависит от соответствующего репозитория (внедряется через конструктор)
        // - Содержит бизнес-логику и валидацию
        // - Преобразует Domain Models в DTOs для API ответов

        /// <summary>Book Service - управление книгами (создание, редактирование, удаление, поиск)</summary>
        services.AddScoped<IBookService, BookService>();

        /// <summary>Author Service - управление авторами</summary>
        services.AddScoped<IAuthorService, AuthorService>();

        /// <summary>Reader Service - управление читателями</summary>
        services.AddScoped<IReaderService, ReaderService>();

        /// <summary>Issue Service - управление выданными книгами (учёт выдачи/возврата)</summary>
        services.AddScoped<IIssueService, IssueService>();

        /// <summary>Publisher Service - управление издателями</summary>
        services.AddScoped<IPublisherService, PublisherService>();

        /// <summary>BookType Service - управление типами/категориями книг</summary>
        services.AddScoped<IBookTypeService, BookTypeService>();

        /// <summary>Analytics Service - статистика и аналитика библиотеки</summary>
        services.AddScoped<IAnalyticsService, AnalyticsService>();

        return services;
    }

    /// <summary>
    /// Регистрирует AutoMapper для трансформации между Domain Models и DTOs.
    /// Сканирует текущую сборку на наличие AutoMapper профилей.
    /// </summary>
    /// <param name="services">IServiceCollection для регистрации сервисов</param>
    /// <returns>IServiceCollection для цепочки вызовов (Method Chaining)</returns>
    /// <remarks>
    /// AutoMapper используется для:
    /// - Трансформации Domain Model → DTO (для API ответов)
    /// - Трансформации DTO → Domain Model (для создания/редактирования)
    /// - Маппинга вложенных объектов (например, Book → BookWithAuthorsDto)
    /// 
    /// Профили определены в сборке Library.Application и автоматически обнаруживаются.
    /// </remarks>
    /// <example>
    /// <code>
    /// builder.Services
    ///     .AddApplicationServices()
    ///     .AddAutoMapperConfiguration();  // Инициализирует AutoMapper
    /// </code>
    /// </example>
    public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
    {
        // ========================================
        // КОНФИГУРАЦИЯ AUTOMAPPER
        // ========================================
        // Сканирует сборку Library.Application на наличие классов, наследующих Profile
        // и автоматически регистрирует все маппинги из текущей сборки
        services.AddAutoMapper(config =>
        {
            config.AddMaps(typeof(ApplicationServiceCollectionExtensions).Assembly);
        });

        return services;
    }
}