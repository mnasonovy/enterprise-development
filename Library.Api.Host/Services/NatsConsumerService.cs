using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Infrastructure.Nats;
using System.Text.Json;

namespace Library.Api.Host.Services;

/// <summary>
/// Фоновый сервис для потребления сообщений из NATS JetStream.
/// Обрабатывает данные и синхронизирует с БД параллельно для всех типов сущностей.
/// </summary>
public sealed class NatsConsumerService(
    INatsConsumer consumer,
    IServiceProvider serviceProvider,
    ILogger<NatsConsumerService> logger)
    : BackgroundService
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Запускает потребителей всех типов данных параллельно при старте приложения.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("NATS consumer service started");

        try
        {
            var bookTypesTask = ConsumeAsync<BookTypeCreateUpdateDto, IBookTypeService>(
                "library_booktypes",
                "library.booktypes",
                (svc, dto) => svc.UpsertAsync(dto),
                stoppingToken);

            var authorsTask = ConsumeAsync<AuthorCreateUpdateDto, IAuthorService>(
                "library_authors",
                "library.authors",
                (svc, dto) => svc.UpsertAsync(dto),
                stoppingToken);

            var publishersTask = ConsumeAsync<PublisherCreateUpdateDto, IPublisherService>(
                "library_publishers",
                "library.publishers",
                (svc, dto) => svc.UpsertAsync(dto),
                stoppingToken);

            var readersTask = ConsumeAsync<ReaderCreateUpdateDto, IReaderService>(
                "library_readers",
                "library.readers",
                (svc, dto) => svc.UpsertAsync(dto),
                stoppingToken);

            var booksTask = ConsumeAsync<BookCreateUpdateDto, IBookService>(
                "library_books",
                "library.books",
                (svc, dto) => svc.UpsertAsync(dto),
                stoppingToken);

            var issuesTask = ConsumeAsync<IssueCreateUpdateDto, IIssueService>(
                "library_issues",
                "library.issues",
                (svc, dto) => svc.UpsertAsync(dto),
                stoppingToken);

            await Task.WhenAll(
                bookTypesTask,
                authorsTask,
                publishersTask,
                readersTask,
                booksTask,
                issuesTask);

            logger.LogInformation("NATS consumer service completed");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("NATS consumer service cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in NATS consumer service");
            throw;
        }
    }

    /// <summary>
    /// Потребляет сообщения из NATS, десериализует и обрабатывает через сервис.
    /// </summary>
    private async Task ConsumeAsync<TDto, TService>(
        string streamName,
        string subject,
        Func<TService, TDto, Task> handle,
        CancellationToken ct)
        where TDto : class
        where TService : class
    {
        try
        {
            logger.LogInformation("Starting consumer for {Subject}", subject);

            await foreach (var bytes in consumer.ConsumeAsync(streamName, subject, ct))
            {
                try
                {
                    var dto = JsonSerializer.Deserialize<TDto>(bytes, _jsonOptions);
                    if (dto is null)
                    {
                        logger.LogWarning("Null {Type} from {Subject}", typeof(TDto).Name, subject);
                        continue;
                    }

                    using var scope = serviceProvider.CreateScope();
                    var service = scope.ServiceProvider.GetRequiredService<TService>();

                    await handle(service, dto);

                    logger.LogInformation("Processed {Type} from {Subject}", typeof(TDto).Name, subject);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error processing {Type} from {Subject}", typeof(TDto).Name, subject);
                }
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Consumer for {Subject} cancelled", subject);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error in consumer for {Subject}", subject);
        }
    }
}
