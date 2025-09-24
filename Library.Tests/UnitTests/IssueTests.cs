using Library.Tests.UnitTests;
using Library.Domain.Models;
using Xunit;

namespace Library.Tests.UnitTests;

/// <summary>
/// Unit tests for querying library data using LINQ over in-memory seed collections.
/// </summary>
/// <remarks>
/// Covers the following scenarios:
/// 1) Issued books ordered by title;
/// 2) Top-5 readers by number of books in a given period (last 6 months);
/// 3) Readers who took books for the longest period, ordered by full name;
/// 4) Top-5 publishers by number of issued books in the last year;
/// 5) Top-5 least popular books in the last year.
/// </remarks>
public class LibraryQueriesTests(DataSeed seed) : IClassFixture<DataSeed>
{
    /// <summary>
    /// Returns distinct issued book titles ordered alphabetically.
    /// </summary>
    [Fact]
    public void IssuedBooks_ShouldBeOrderedByTitle()
    {
        // Act
        var result = seed.Issues
            .Select(i => i.Book.Title)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        // Expected: only titles that actually appear in Issues, sorted A→Z
        var expected = new[]
        {
            "A Month in the Country",
            "Ancient Philosophy",
            "Children's Tales",
            "Collected Works",
            "Crime and Punishment",
            "Doctor Zhivago",
            "Eugene Onegin",
            "Literary Encyclopedia",
            "Modern Programming",
            "Modern Review",
            "The Enchanted Forest",
            "The Master and Margarita",
            "War and Peace"
        };

        // Assert
        Assert.Equal(expected, result);
    }

    /// <summary>
    /// Returns the top 5 readers who read the most books within the last 6 months.
    /// </summary>
    [Fact]
    public void Top5Readers_ByBooksReadInLast6Months()
    {
        // Arrange
        var start = DateTime.Today.AddMonths(-6);
        var end = DateTime.Today;

        // Act
        var result = seed.Issues
            .Where(i => i.IssueDate >= start && i.IssueDate <= end)
            .GroupBy(i => i.Reader)
            .Select(g => new { FullName = g.Key.FullName, BooksCount = g.Count() })
            .OrderByDescending(x => x.BooksCount)
            .ThenBy(x => x.FullName)
            .Take(5)
            .ToArray();

        // Expected (precomputed for the seeded data)
        var expected = new[]
        {
            new { FullName = "Ivan Petrov",     BooksCount = 3 },
            new { FullName = "Sergey Smirnov",  BooksCount = 3 },
            new { FullName = "Anna Ivanova",    BooksCount = 2 },
            new { FullName = "Dmitry Volkov",   BooksCount = 2 },
            new { FullName = "Elena Popova",    BooksCount = 1 }
        };

        // Assert
        Assert.Equal(expected.Length, result.Length);
        for (var i = 0; i < expected.Length; i++)
        {
            Assert.Equal(expected[i].FullName, result[i].FullName);
            Assert.Equal(expected[i].BooksCount, result[i].BooksCount);
        }
    }

    /// <summary>
    /// Returns readers with their maximum issue period (in days), ordered by full name.
    /// </summary>
    [Fact]
    public void Readers_WithLongestIssuePeriod_OrderedByName()
    {
        // Act
        var result = seed.Issues
            .GroupBy(i => i.Reader)
            .Select(g => new { Reader = g.Key, MaxDays = g.Max(i => i.DaysCount) })
            .Where(r => r.MaxDays > 0)
            .OrderBy(r => r.Reader.FullName)
            .ToList();

        // Expected (precomputed for the seeded data; A→Z by FullName)
        var expected = new List<(string FullName, int MaxDays)>
        {
            ("Alexey Mikhailov", 14),
            ("Anna Ivanova", 30),
            ("Dmitry Volkov", 10),
            ("Elena Popova", 7),
            ("Irina Sidorova", 21),
            ("Ivan Petrov", 14),
            ("Olga Sokolova", 365),
            ("Pavel Kuznetsov", 10),
            ("Sergey Smirnov", 14)
        };

        // Assert
        Assert.Equal(expected.Count, result.Count);
        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].FullName, result[i].Reader.FullName);
            Assert.Equal(expected[i].MaxDays, result[i].MaxDays);
        }
    }

    /// <summary>
    /// Returns the top 5 publishers by number of issued books over the last year.
    /// </summary>
    [Fact]
    public void Top5Publishers_ByIssuedBooksInLastYear()
    {
        // Act
        var result = seed.Issues
            .Where(i => i.IssueDate >= DateTime.Today.AddYears(-1))
            .GroupBy(i => i.Book.Publisher)
            .Select(g => new { Publisher = g.Key, IssuesCount = g.Count() })
            .OrderByDescending(x => x.IssuesCount)
            .ThenBy(x => x.Publisher.Name)
            .Take(5)
            .ToList();

        // Expected (precomputed for the seeded data)
        var expected = new List<(string Publisher, int IssuesCount)>
        {
            ("Eksmo", 4),
            ("AST", 3),
            ("MIF", 3),
            ("Piter", 3),
            ("Drofa", 2)
        };

        // Assert
        Assert.Equal(expected.Count, result.Count);
        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Publisher, result[i].Publisher.Name);
            Assert.Equal(expected[i].IssuesCount, result[i].IssuesCount);
        }
    }

    /// <summary>
    /// Returns the top 5 least popular books (by issue count) in the last year.
    /// </summary>
    [Fact]
    public void Top5LeastPopularBooks_InLastYear()
    {
        // Arrange
        var since = DateTime.Today.AddYears(-1);

        // Act
        var result = seed.Issues
            .Where(i => i.IssueDate >= since)
            .GroupBy(i => i.Book)
            .Select(g => new { Book = g.Key, IssuesCount = g.Count() })
            .OrderBy(x => x.IssuesCount)
            .ThenBy(x => x.Book.Title)
            .Take(5)
            .ToList();

        // Expected (precomputed for the seeded data)
        var expected = new List<(string Title, int IssuesCount)>
        {
            ("A Month in the Country", 1),
            ("Collected Works",        1),
            ("Doctor Zhivago",         1),
            ("Eugene Onegin",          1),
            ("Literary Encyclopedia",  1)
        };

        // Assert
        Assert.Equal(expected.Count, result.Count);
        for (var i = 0; i < expected.Count; i++)
        {
            Assert.Equal(expected[i].Title, result[i].Book.Title);
            Assert.Equal(expected[i].IssuesCount, result[i].IssuesCount);
        }
    }
}
