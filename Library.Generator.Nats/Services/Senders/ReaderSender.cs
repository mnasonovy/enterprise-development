using Bogus;
using Library.Application.Contracts.Readers;
using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Генератор и отправитель данных читателей в NATS JetStream.
/// Генерирует 100 читателей с Bogus и публикует в очередь для синхронизации.
/// </summary>
public sealed class ReaderSender(INatsProducer producer, ILogger<ReaderSender> logger)
    : BaseSeedDataSender<ReaderCreateUpdateDto>(producer, logger)
{
    private const int ReadersCountToGenerate = 100;

    /// <summary>
    /// Генерирует читателей и отправляет в NATS батчами.
    /// </summary>
    public override async Task SendAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Generating {Count} readers with Bogus...", ReadersCountToGenerate);

            var readers = GenerateReadersWithBogus(ReadersCountToGenerate);

            if (readers.Count == 0)
            {
                _logger.LogWarning("No readers generated");
                return;
            }

            await SendInBatchesAsync(readers, "library.readers", "Reader", ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate and send readers");
            throw;
        }
    }

    /// <summary>
    /// Генерирует читателей с использованием Bogus (100 штук).
    /// Каждому читателю присваиваются ФИ, адрес, телефон и дата регистрации.
    /// ID генерируется автоматически на сервере.
    /// </summary>
    private static List<ReaderCreateUpdateDto> GenerateReadersWithBogus(int count)
    {
        var readerFaker = new Faker<ReaderCreateUpdateDto>("ru")
            .RuleFor(r => r.FullName, f => f.Person.FullName)
            .RuleFor(r => r.Address, f => f.Address.FullAddress())
            .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber("+7 (9##) ###-##-##"))
            .RuleFor(r => r.RegistrationDate, f => f.Date.Between(
                new DateTime(2020, 1, 1),
                DateTime.UtcNow));

        return readerFaker.Generate(count);
    }

    /// <summary>
    /// Логирует отправку читателя в NATS.
    /// </summary>
    protected override void LogItemSent(string entityName, int sent, int total, object item)
    {
        if (item is ReaderCreateUpdateDto dto)
        {
            _logger.LogInformation(
                "Reader sent to NATS: [{Sent}/{Total}] {FullName}",
                sent, total, dto.FullName);
        }
    }
}
