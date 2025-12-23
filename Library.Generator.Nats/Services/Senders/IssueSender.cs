using Bogus;
using Library.Application.Contracts.Issues;
using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Генератор и отправитель данных выдач в NATS JetStream.
/// Генерирует 300 выдач с Bogus и публикует в очередь для синхронизации.
/// </summary>
public sealed class IssueSender(INatsProducer producer, ILogger<IssueSender> logger)
    : BaseSeedDataSender<IssueCreateUpdateDto>(producer, logger)
{
    private const int IssuesCountToGenerate = 300;

    /// <summary>
    /// Генерирует выдачи и отправляет в NATS батчами.
    /// </summary>
    public override async Task SendAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Generating {Count} issues with Bogus...", IssuesCountToGenerate);

            var issues = GenerateIssuesWithBogus(IssuesCountToGenerate);

            if (issues is null || issues.Count == 0)
            {
                _logger.LogWarning("No issues generated");
                return;
            }

            await SendInBatchesAsync(issues, "library.issues", "Issue", ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate and send issues");
            throw;
        }
    }

    /// <summary>
    /// Генерирует выдачи с использованием Bogus (300 штук).
    /// Каждой выдаче присваиваются случайные книга, читатель, даты выдачи и возврата.
    /// 85% выдач содержат дату возврата, 15% еще в процессе.
    /// </summary>
    private static List<IssueCreateUpdateDto> GenerateIssuesWithBogus(int count)
    {
        var issueFaker = new Faker<IssueCreateUpdateDto>("ru")
            .RuleFor(i => i.Id, (f, u) => f.IndexFaker + 1)
            .RuleFor(i => i.BookId, f => f.Random.Int(1, 200))
            .RuleFor(i => i.ReaderId, f => f.Random.Int(1, 100))
            .RuleFor(i => i.IssueDate, f => f.Date.Between(
                new DateTime(2023, 1, 1),
                DateTime.UtcNow.AddMonths(-1)))
            .RuleFor(i => i.DaysCount, f => f.Random.Int(14, 30))
            .RuleFor(i => i.ReturnDate, (f, u) =>
                f.Random.Bool(0.85f)
                    ? u.IssueDate.AddDays(u.DaysCount + f.Random.Int(-5, 15))
                    : null);

        return issueFaker.Generate(count);
    }

    /// <summary>
    /// Логирует отправку выдачи в NATS.
    /// </summary>
    protected override void LogItemSent(string entityName, int sent, int total, object item)
    {
        if (item is IssueCreateUpdateDto dto)
        {
            var status = dto.ReturnDate.HasValue ? "Returned" : "Active";
            _logger.LogInformation(
                "Issue sent to NATS: [{Sent}/{Total}] {Id} BookId={BookId} ReaderId={ReaderId} Days={Days} Status={Status}",
                sent, total, dto.Id, dto.BookId, dto.ReaderId, dto.DaysCount, status);
        }
    }
}
