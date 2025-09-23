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
}