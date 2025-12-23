using Library.Generator.Nats.Services.Senders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services;

/// <summary>
/// Фоновый сервис для генерации и отправки данных в NATS JetStream.
/// Оркестрирует отправку сущностей в порядке зависимостей:
/// BookTypes → Authors → Publishers → Readers → Books → Issues
/// </summary>
public sealed class DataGeneratorService(
    BookTypeSender bookTypeSender,
    AuthorSender authorSender,
    PublisherSender publisherSender,
    ReaderSender readerSender,
    BookSender bookSender,
    IssueSender issueSender,
    ILogger<DataGeneratorService> logger)
    : BackgroundService
{
    private const int DelayBetweenSenderMs = 2000;
    private const int MaxRetries = 3;
    private const int InitialDelayMs = 500;

    /// <summary>
    /// Запускает генерацию и отправку данных при старте приложения.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Data generator started");

        try
        {
            await SendWithRetryAsync(bookTypeSender, "BookTypes", stoppingToken);
            await Task.Delay(DelayBetweenSenderMs, stoppingToken);

            await SendWithRetryAsync(authorSender, "Authors", stoppingToken);
            await Task.Delay(DelayBetweenSenderMs, stoppingToken);

            await SendWithRetryAsync(publisherSender, "Publishers", stoppingToken);
            await Task.Delay(DelayBetweenSenderMs, stoppingToken);

            await SendWithRetryAsync(readerSender, "Readers", stoppingToken);
            await Task.Delay(DelayBetweenSenderMs, stoppingToken);

            await SendWithRetryAsync(bookSender, "Books", stoppingToken);
            await Task.Delay(DelayBetweenSenderMs, stoppingToken);

            await SendWithRetryAsync(issueSender, "Issues", stoppingToken);

            logger.LogInformation("Data generator finished successfully");
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("Data generator cancelled");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error in data generator");
        }
    }

    /// <summary>
    /// Отправляет данные с retry логикой (максимум 3 попытки с экспоненциальной задержкой).
    /// </summary>
    private async Task SendWithRetryAsync(
        ISeedDataSender sender,
        string entityName,
        CancellationToken ct)
    {
        var attempt = 0;

        while (attempt < MaxRetries)
        {
            try
            {
                logger.LogInformation(
                    "Sending {Entity} (attempt {Attempt}/{Max})",
                    entityName, attempt + 1, MaxRetries);

                await sender.SendAsync(ct);

                logger.LogInformation("{Entity} sent successfully", entityName);
                return;
            }
            catch (Exception ex)
            {
                attempt++;

                if (attempt >= MaxRetries)
                {
                    logger.LogError(
                        ex,
                        "Failed to send {Entity} after {Retries} attempts. Skipping...",
                        entityName, MaxRetries);
                    return;
                }

                var delayMs = InitialDelayMs * (int)Math.Pow(2, attempt - 1);
                logger.LogWarning(
                    "Retry {Attempt} for {Entity} in {Delay}ms...",
                    attempt, entityName, delayMs);

                await Task.Delay(delayMs, ct);
            }
        }
    }
}
