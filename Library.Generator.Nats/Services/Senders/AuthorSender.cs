using Library.Application.Contracts.Authors;
using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Отправитель данных авторов из seed файла в NATS JetStream.
/// Загружает авторов из JSON и публикует их в очередь для синхронизации.
/// </summary>
public sealed class AuthorSender(INatsProducer producer, ILogger<AuthorSender> logger)
    : BaseSeedDataSender<AuthorCreateUpdateDto>(producer, logger)
{
    /// <summary>
    /// Загружает авторов из seed файла и отправляет в NATS батчами.
    /// </summary>
    public override async Task SendAsync(CancellationToken ct)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Seed", "authors.json");

        if (!File.Exists(path))
        {
            _logger.LogWarning("Seed file not found: {Path}", path);
            return;
        }

        var authors = await LoadDataFromFileAsync(path, ct);

        if (authors is null || authors.Count == 0)
        {
            _logger.LogWarning("No authors found in seed file: {Path}", path);
            return;
        }

        await SendInBatchesAsync(authors, "library.authors", "Author", ct);
    }

    /// <summary>
    /// Загружает список авторов из JSON файла.
    /// </summary>
    private async Task<List<AuthorCreateUpdateDto>?> LoadDataFromFileAsync(
        string path,
        CancellationToken ct)
    {
        try
        {
            var json = await File.ReadAllTextAsync(path, ct);
            return System.Text.Json.JsonSerializer.Deserialize<List<AuthorCreateUpdateDto>>(
                json, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load authors from seed file: {Path}", path);
            throw;
        }
    }

    /// <summary>
    /// Логирует отправку автора в NATS.
    /// </summary>
    protected override void LogItemSent(string entityName, int sent, int total, object item)
    {
        if (item is AuthorCreateUpdateDto dto)
        {
            _logger.LogInformation(
                "Author sent to NATS: [{Sent}/{Total}] {LastName}",
                sent, total, dto.LastName);
        }
    }
}
