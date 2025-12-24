using Library.Application.Contracts.Publishers;
using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Отправитель данных издателей из seed файла в NATS JetStream.
/// Загружает издателей из JSON и публикует в очередь для синхронизации.
/// </summary>
public sealed class PublisherSender(INatsProducer producer, ILogger<PublisherSender> logger)
    : BaseSeedDataSender<PublisherCreateUpdateDto>(producer, logger)
{
    /// <summary>
    /// Загружает издателей из seed файла и отправляет в NATS батчами.
    /// </summary>
    public override async Task SendAsync(CancellationToken ct)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Seed", "publishers.json");

        if (!File.Exists(path))
        {
            _logger.LogWarning("Seed file not found: {Path}", path);
            return;
        }

        var publishers = await LoadDataFromFileAsync(path, ct);

        if (publishers is null || publishers.Count == 0)
        {
            _logger.LogWarning("No publishers found in seed file: {Path}", path);
            return;
        }

        await SendInBatchesAsync(publishers, "library.publishers", "Publisher", ct);
    }

    /// <summary>
    /// Загружает список издателей из JSON файла.
    /// </summary>
    private async Task<List<PublisherCreateUpdateDto>?> LoadDataFromFileAsync(
        string path,
        CancellationToken ct)
    {
        try
        {
            var json = await File.ReadAllTextAsync(path, ct);
            return System.Text.Json.JsonSerializer.Deserialize<List<PublisherCreateUpdateDto>>(
                json, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load publishers from seed file: {Path}", path);
            throw;
        }
    }

    /// <summary>
    /// Логирует отправку издателя в NATS.
    /// </summary>
    protected override void LogItemSent(string entityName, int sent, int total, object item)
    {
        if (item is PublisherCreateUpdateDto dto)
        {
            _logger.LogInformation(
                "Publisher sent to NATS: [{Sent}/{Total}] {Name}",
                sent, total, dto.Name);
        }
    }
}
