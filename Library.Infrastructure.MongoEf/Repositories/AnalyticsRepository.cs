using Library.Application.Contracts.Analytics;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;
using Library.Infrastructure.MongoEf.Database;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.MongoEf.Repositories;

public class AnalyticsRepository(MongoDbContext context) : IAnalyticsRepository
{
    public async Task<IReadOnlyList<string>> GetIssuedBooksOrderedByTitleAsync()
    {
        var issues = await context.Issues
            .AsNoTracking()
            .ToListAsync();

        var bookIds = issues
            .Select(i => i.BookId)
            .Distinct()
            .ToList();

        var books = await context.Books
            .AsNoTracking()
            .Where(b => bookIds.Contains(b.Id))
            .Select(b => b.Title)
            .OrderBy(t => t)
            .ToListAsync();

        return books.AsReadOnly();
    }

    public async Task<IReadOnlyList<TopReaderDto>> GetTopReadersAsync()
    {
        var last6Months = DateTime.UtcNow.AddMonths(-6);

        var issues = await context.Issues
            .AsNoTracking()
            .Where(i => i.IssueDate >= last6Months)
            .ToListAsync();

        var result = issues
            .GroupBy(i => i.ReaderId)
            .Select(g => new { ReaderId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var readerIds = result.Select(x => x.ReaderId).ToList();
        var readers = await context.Readers
            .AsNoTracking()
            .Where(r => readerIds.Contains(r.Id))
            .ToListAsync();

        var topReaders = result
            .Join(readers, r => r.ReaderId, reader => reader.Id,
                (r, reader) => new TopReaderDto
                {
                    FullName = reader.FullName,
                    CountBooks = r.Count
                })
            .ToList();

        return topReaders.AsReadOnly();
    }

    public async Task<IReadOnlyList<ReaderDaysCountDto>> GetReadersByDaysCountAsync()
    {
        var readers = await context.Readers
            .AsNoTracking()
            .ToListAsync();

        var issues = await context.Issues
            .AsNoTracking()
            .ToListAsync();

        var result = readers
            .Select(r => new ReaderDaysCountDto
            {
                FullName = r.FullName,
                CountDays = issues.Where(i => i.ReaderId == r.Id).Sum(i => i.DaysCount)
            })
            .OrderBy(x => x.FullName)
            .ToList();

        return result.AsReadOnly();
    }

    public async Task<IReadOnlyList<TopPublisherDto>> GetTopPublishersLastYearAsync()
    {
        var lastYear = DateTime.UtcNow.AddYears(-1);

        var issues = await context.Issues
            .AsNoTracking()
            .Where(i => i.IssueDate >= lastYear)
            .ToListAsync();

        var books = await context.Books
            .AsNoTracking()
            .ToListAsync();

        var publisherCounts = issues
            .Join(books, issue => issue.BookId, book => book.Id,
                (issue, book) => new { book.PublisherId })
            .GroupBy(x => x.PublisherId)
            .Select(g => new { PublisherId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        var publisherIds = publisherCounts.Select(x => x.PublisherId).ToList();
        var publishers = await context.Publishers
            .AsNoTracking()
            .Where(p => publisherIds.Contains(p.Id))
            .ToListAsync();

        var result = publisherCounts
            .Join(publishers, pc => pc.PublisherId, pub => pub.Id,
                (pc, pub) => new TopPublisherDto
                {
                    PublisherName = pub.Name,
                    CountBooks = pc.Count
                })
            .ToList();

        return result.AsReadOnly();
    }

    public async Task<IReadOnlyList<TopBookDto>> GetTopPopularBooksLastYearAsync()
    {
        var lastYear = DateTime.UtcNow.AddYears(-1);

        var issues = await context.Issues
            .AsNoTracking()
            .Where(i => i.IssueDate >= lastYear)
            .ToListAsync();

        var books = await context.Books
            .AsNoTracking()
            .ToListAsync();

        var bookCounts = issues
            .GroupBy(i => i.BookId)
            .Select(g => new { BookId = g.Key, Count = g.Count() })
            .OrderBy(x => x.Count)
            .Take(5)
            .ToList();

        var bookIds = bookCounts.Select(x => x.BookId).ToList();
        var selectedBooks = books
            .Where(b => bookIds.Contains(b.Id))
            .ToList();

        var result = bookCounts
            .Join(selectedBooks, bc => bc.BookId, book => book.Id,
                (bc, book) => new TopBookDto
                {
                    Title = book.Title,
                    TimesIssued = bc.Count
                })
            .ToList();

        return result.AsReadOnly();
    }
}
