using Library.Application.Contracts.BookTypes;
using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Отправитель данных типов книг из seed файла в NATS JetStream.
/// Загружает типы книг из JSON и публикует в очередь для синхронизации.
/// </summary>
public sealed class BookTypeSender(INatsProducer producer, ILogger<BookTypeSender> logger)
    : BaseSeedDataSender<BookTypeCreateUpdateDto>(producer, logger)
{
    /// <summary>
    /// Загружает типы книг из seed файла и отправляет в NATS батчами.
    /// </summary>
    public override async Task SendAsync(CancellationToken ct)
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Seed", "booktype.json");

        if (!File.Exists(path))
        {
            _logger.LogWarning("Seed file not found: {Path}", path);
            return;
        }

        var bookTypes = await LoadDataFromFileAsync(path, ct);

        if (bookTypes is null || bookTypes.Count == 0)
        {
            _logger.LogWarning("No book types found in seed file: {Path}", path);
            return;
        }

        await SendInBatchesAsync(bookTypes, "library.booktypes", "BookType", ct);
    }

    /// <summary>
    /// Загружает список типов книг из JSON файла.
    /// </summary>
    private async Task<List<BookTypeCreateUpdateDto>?> LoadDataFromFileAsync(
        string path,
        CancellationToken ct)
    {
        try
        {
            var json = await File.ReadAllTextAsync(path, ct);
            return System.Text.Json.JsonSerializer.Deserialize<List<BookTypeCreateUpdateDto>>(
                json, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load book types from seed file: {Path}", path);
            throw;
        }
    }

    /// <summary>
    /// Логирует отправку типа книги в NATS.
    /// </summary>
    protected override void LogItemSent(string entityName, int sent, int total, object item)
    {
        if (item is BookTypeCreateUpdateDto dto)
        {
            _logger.LogInformation(
                "BookType sent to NATS: [{Sent}/{Total}] {Name}",
                sent, total, dto.Name);
        }
    }
}
