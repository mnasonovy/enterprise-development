using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Интерфейс для отправки данных в NATS.
/// </summary>
public interface ISeedDataSender
{
    /// <summary>
    /// Отправляет данные в NATS JetStream.
    /// </summary>
    public Task SendAsync(CancellationToken ct);
}

/// <summary>
/// Базовый класс для отправки данных в NATS батчами.
/// Обеспечивает общую логику отправки с обработкой ошибок и логированием.
/// </summary>
public abstract class BaseSeedDataSender<TDto>(INatsProducer producer, ILogger logger) : ISeedDataSender
    where TDto : class
{
    protected readonly INatsProducer _producer = producer;
    protected readonly ILogger _logger = logger;
    protected const int BatchSize = 5;
    protected const int DelayBetweenBatchesMs = 200;

    protected static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    /// <summary>
    /// Отправляет данные в NATS (реализуется в наследниках).
    /// </summary>
    public abstract Task SendAsync(CancellationToken ct);

    /// <summary>
    /// Отправляет список данных батчами с обработкой ошибок.
    /// </summary>
    protected async Task SendInBatchesAsync<T>(
        List<T> items,
        string subject,
        string entityName,
        CancellationToken ct)
        where T : class
    {
        _logger.LogInformation(
            "Sending {Count} {Entity} to NATS in batches of {BatchSize}...",
            items.Count, entityName, BatchSize);

        var totalSent = 0;

        for (var i = 0; i < items.Count; i += BatchSize)
        {
            ct.ThrowIfCancellationRequested();

            var batch = items.Skip(i).Take(BatchSize).ToList();

            foreach (var item in batch)
            {
                try
                {
                    await _producer.PublishAsync(subject, item, ct);
                    totalSent++;

                    LogItemSent(entityName, totalSent, items.Count, item);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Failed to send {Entity}, skipping...",
                        entityName);
                }
            }

            if (i + BatchSize < items.Count)
            {
                await Task.Delay(DelayBetweenBatchesMs, ct);
            }
        }

        _logger.LogInformation(
            "{Entity} sending completed. Total sent: {Total}/{Count}",
            entityName, totalSent, items.Count);
    }

    /// <summary>
    /// Логирует отправку одного элемента (реализуется в наследниках).
    /// </summary>
    protected abstract void LogItemSent(string entityName, int sent, int total, object item);
}
