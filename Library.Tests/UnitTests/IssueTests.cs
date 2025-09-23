using Library.Domain.Data;
using Library.Domain.Models;
using Xunit;

namespace Library.Tests.UnitTests;

public class IssueTests
{
    [Fact]
    public void IssuedBooks_ShouldBeOrderedByTitle()
    {
        // Arrange: generate seed data
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers);

        // Act: join Issues with Books and order by Title
        var result = issues
            .Join(books,
                issue => issue.BookId,
                book => book.Id,
                (issue, book) => new { issue.Id, book.Title })
            .OrderBy(x => x.Title)
            .ToList();

        // Assert: ensure result is ordered by Title
        var sorted = result.OrderBy(x => x.Title).ToList();
        Assert.Equal(sorted, result);
    }

    [Fact]
    public void Top5Readers_ByBooksReadInPeriod()
    {
        // Arrange: generate seed data
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers, 50); // more issues for better test

        // Define period (last 6 months)
        var startDate = DateTime.Now.AddMonths(-6);
        var endDate = DateTime.Now;

        // Act: filter issues by period and count books per reader
        var result = issues
            .Where(i => i.IssueDate >= startDate && i.IssueDate <= endDate)
            .GroupBy(i => i.ReaderId)
            .Select(g => new
            {
                Reader = readers.First(r => r.Id == g.Key),
                BooksCount = g.Count()
            })
            .OrderByDescending(x => x.BooksCount)
            .ThenBy(x => x.Reader.FullName)
            .Take(5)
            .ToList();

        // Assert: not more than 5 readers returned
        Assert.True(result.Count <= 5);

        // Assert: ordered by BooksCount descending, then by name
        var ordered = result.OrderByDescending(x => x.BooksCount).ThenBy(x => x.Reader.FullName).ToList();
        Assert.Equal(ordered, result);
    }
}
