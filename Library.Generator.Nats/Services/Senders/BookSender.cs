using Bogus;
using Library.Application.Contracts.Books;
using Library.Generator.Nats.Producer;
using Microsoft.Extensions.Logging;

namespace Library.Generator.Nats.Services.Senders;

/// <summary>
/// Генератор и отправитель данных книг в NATS JetStream.
/// Генерирует 200 книг с Bogus и публикует в очередь для синхронизации.
/// </summary>
public sealed class BookSender(INatsProducer producer, ILogger<BookSender> logger)
    : BaseSeedDataSender<BookCreateUpdateDto>(producer, logger)
{
    private const int BooksCountToGenerate = 200;
    private const int MaxAuthorsPerBook = 3;

    /// <summary>
    /// Генерирует книги и отправляет в NATS батчами.
    /// </summary>
    public override async Task SendAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Generating {Count} books with Bogus...", BooksCountToGenerate);

            var books = GenerateBooksWithBogus(BooksCountToGenerate);

            if (books is null || books.Count == 0)
            {
                _logger.LogWarning("No books generated");
                return;
            }

            await SendInBatchesAsync(books, "library.books", "Book", ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate and send books");
            throw;
        }
    }

    /// <summary>
    /// Генерирует книги с использованием Bogus (200 штук).
    /// Названия генерируются из 3-5 русских слов с заглавными буквами.
    /// Каждой книге присваиваются случайные авторы, тип и издатель.
    /// </summary>
    private static List<BookCreateUpdateDto> GenerateBooksWithBogus(int count)
    {
        var bookFaker = new Faker<BookCreateUpdateDto>("ru")
            .RuleFor(b => b.Id, (f, u) => f.IndexFaker + 1)
            .RuleFor(b => b.Title, f =>
                string.Join(" ",
                    f.Lorem.Words(f.Random.Int(3, 5))
                        .Select(word => char.ToUpper(word[0]) + word[1..])))
            .RuleFor(b => b.Year, f => f.Random.Int(1800, DateTime.UtcNow.Year))
            .RuleFor(b => b.AlphabetCode, f =>
                f.Random.Bool(0.85f)
                    ? $"{f.Random.Char('А', 'Я')}{f.Random.Char('А', 'Я')}{f.Random.Int(1, 999)}"
                    : null)
            .RuleFor(b => b.BookTypeId, f => f.Random.Int(1, 5))
            .RuleFor(b => b.PublisherId, f => f.Random.Int(1, 20))
            .RuleFor(b => b.AuthorIds, f =>
                [.. Enumerable.Range(0, f.Random.Int(1, MaxAuthorsPerBook))
                    .Select(_ => f.Random.Int(1, 100))
                    .Distinct()]);

        return bookFaker.Generate(count);
    }

    /// <summary>
    /// Логирует отправку книги в NATS.
    /// </summary>
    protected override void LogItemSent(string entityName, int sent, int total, object item)
    {
        if (item is BookCreateUpdateDto dto)
        {
            var authorCount = dto.AuthorIds.Count;
            _logger.LogInformation(
                "Book sent to NATS: [{Sent}/{Total}] {Id} \"{Title}\" ({Year}, {AuthorCount} author(s))",
                sent, total, dto.Id, dto.Title, dto.Year, authorCount);
        }
    }
}
