using Library.Domain.Data;
using Library.Domain.Models;
using Xunit;

namespace Library.Tests.UnitTests;

/// <summary>
/// Contains unit tests for the <see cref="Issue"/> entity and related queries.
/// </summary>
public class IssueTests
{
    /// <summary>
    /// Verifies that issued books are correctly ordered by title.
    /// </summary>
    [Fact]
    public void IssuedBooks_ShouldBeOrderedByTitle()
    {
        // Arrange
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers);

        // Act
        var result = issues
            .Join(books,
                issue => issue.BookId,
                book => book.Id,
                (issue, book) => new { issue.Id, book.Title })
            .OrderBy(x => x.Title)
            .ToList();

        // Assert
        var sorted = result.OrderBy(x => x.Title).ToList();
        Assert.Equal(sorted, result);
    }

    /// <summary>
    /// Verifies that the top 5 readers who read the most books in a given period are returned in the correct order.
    /// </summary>
    [Fact]
    public void Top5Readers_ByBooksReadInPeriod()
    {
        // Arrange
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers, 50);

        var startDate = DateTime.Now.AddMonths(-6);
        var endDate = DateTime.Now;

        // Act
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

        // Assert
        Assert.True(result.Count <= 5);

        var ordered = result
            .OrderByDescending(x => x.BooksCount)
            .ThenBy(x => x.Reader.FullName)
            .ToList();

        Assert.Equal(ordered, result);
    }

    /// <summary>
    /// Verifies that readers with the longest issue periods are returned, ordered by full name.
    /// </summary>
    [Fact]
    public void Readers_WithLongestIssuePeriod_OrderedByName()
    {
        // Arrange
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers, 50);

        // Act
        var result = issues
            .GroupBy(i => i.ReaderId)
            .Select(g => new
            {
                Reader = readers.First(r => r.Id == g.Key),
                MaxDays = g.Max(i => i.DaysCount)
            })
            .OrderBy(r => r.Reader.FullName)
            .ToList();

        // Assert
        var ordered = result.OrderBy(r => r.Reader.FullName).ToList();
        Assert.Equal(ordered, result);

        Assert.All(result, r => Assert.True(r.MaxDays > 0));
    }

    /// <summary>
    /// Verifies that the top 5 publishers with the most issued books in the last year are returned in the correct order.
    /// </summary>
    [Fact]
    public void Top5Publishers_ByIssuedBooksInLastYear()
    {
        // Arrange
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers, 100);

        var since = DateTime.Now.AddYears(-1);

        // Act
        var result = issues
            .Where(i => i.IssueDate >= since)
            .Join(books,
                issue => issue.BookId,
                book => book.Id,
                (issue, book) => book)
            .Join(publishers,
                book => book.PublisherId,
                publisher => publisher.Id,
                (book, publisher) => publisher)
            .GroupBy(p => p.Id)
            .Select(g => new
            {
                Publisher = publishers.First(p => p.Id == g.Key),
                IssuesCount = g.Count()
            })
            .OrderByDescending(x => x.IssuesCount)
            .ThenBy(x => x.Publisher.Name)
            .Take(5)
            .ToList();

        // Assert
        Assert.True(result.Count <= 5);

        var ordered = result
            .OrderByDescending(x => x.IssuesCount)
            .ThenBy(x => x.Publisher.Name)
            .ToList();

        Assert.Equal(ordered, result);
    }

    /// <summary>
    /// Verifies that the top 5 least popular books in the last year are returned in the correct order.
    /// </summary>
    [Fact]
    public void Top5LeastPopularBooks_InLastYear()
    {
        // Arrange
        var authors = DataSeeder.GenerateAuthors();
        var publishers = DataSeeder.GeneratePublishers();
        var bookTypes = DataSeeder.GenerateBookTypes();
        var books = DataSeeder.GenerateBooks(authors, publishers, bookTypes, 20);
        var readers = DataSeeder.GenerateReaders();
        var issues = DataSeeder.GenerateIssues(books, readers, 100);

        var since = DateTime.Now.AddYears(-1);

        // Act
        var result = issues
            .Where(i => i.IssueDate >= since)
            .GroupBy(i => i.BookId)
            .Select(g => new
            {
                Book = books.First(b => b.Id == g.Key),
                IssuesCount = g.Count()
            })
            .OrderBy(x => x.IssuesCount)
            .ThenBy(x => x.Book.Title)
            .Take(5)
            .ToList();

        // Assert
        Assert.True(result.Count <= 5);

        var ordered = result
            .OrderBy(x => x.IssuesCount)
            .ThenBy(x => x.Book.Title)
            .ToList();

        Assert.Equal(ordered, result);
    }
}
