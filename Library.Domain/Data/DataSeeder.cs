using Bogus;
using Library.Domain.Models;

namespace Library.Domain.Data;

/// <summary>
/// Provides methods for generating test data for the library system using the Bogus library.
/// </summary>
public static class DataSeeder
{
    /// <summary>
    /// Generates a list of authors.
    /// </summary>
    /// <param name="count">Number of authors to generate (default: 10).</param>
    /// <returns>List of generated <see cref="Author"/> objects.</returns>
    public static List<Author> GenerateAuthors(int count = 10)
    {
        var faker = new Faker<Author>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.Initials, f => $"{f.Name.FirstName()[0]}.")
            .RuleFor(a => a.LastName, f => f.Name.LastName());

        return faker.Generate(count);
    }

    /// <summary>
    /// Generates a list of publishers.
    /// </summary>
    /// <param name="count">Number of publishers to generate (default: 10).</param>
    /// <returns>List of generated <see cref="Publisher"/> objects.</returns>
    public static List<Publisher> GeneratePublishers(int count = 10)
    {
        var faker = new Faker<Publisher>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Company.CompanyName());

        return faker.Generate(count);
    }

    /// <summary>
    /// Generates a list of book types.
    /// </summary>
    /// <param name="count">Number of book types to generate (default: 5).</param>
    /// <returns>List of generated <see cref="BookType"/> objects.</returns>
    public static List<BookType> GenerateBookTypes(int count = 5)
    {
        var faker = new Faker<BookType>()
            .RuleFor(bt => bt.Id, f => f.IndexFaker + 1)
            .RuleFor(bt => bt.Name, f => f.Commerce.Categories(1)[0]);

        return faker.Generate(count);
    }

    /// <summary>
    /// Generates a list of books with references to authors, publishers, and book types.
    /// </summary>
    /// <param name="authors">List of authors to assign to books.</param>
    /// <param name="publishers">List of publishers to assign to books.</param>
    /// <param name="bookTypes">List of book types to assign to books.</param>
    /// <param name="count">Number of books to generate (default: 10).</param>
    /// <returns>List of generated <see cref="Book"/> objects.</returns>
    public static List<Book> GenerateBooks(
        List<Author> authors,
        List<Publisher> publishers,
        List<BookType> bookTypes,
        int count = 10)
    {
        var faker = new Faker<Book>()
            .RuleFor(b => b.Id, f => f.IndexFaker + 1)
            .RuleFor(b => b.AlphabetCode, f => f.Random.String2(5, "ABCDEFGHIJKLMNOPQRSTUVWXYZ"))
            .RuleFor(b => b.Title, f => f.Lorem.Sentence(3, 2))
            .RuleFor(b => b.Year, f => f.Date.Past(20).Year)
            .RuleFor(b => b.BookTypeId, f => f.PickRandom(bookTypes).Id)
            .RuleFor(b => b.PublisherId, f => f.PickRandom(publishers).Id)
            .RuleFor(b => b.AuthorIds,
                f => f.PickRandom(authors.Select(a => a.Id), f.Random.Int(1, 3)).ToList());

        return faker.Generate(count);
    }

    /// <summary>
    /// Generates a list of readers.
    /// </summary>
    /// <param name="count">Number of readers to generate (default: 10).</param>
    /// <returns>List of generated <see cref="Reader"/> objects.</returns>
    public static List<Reader> GenerateReaders(int count = 10)
    {
        var faker = new Faker<Reader>()
            .RuleFor(r => r.Id, f => f.IndexFaker + 1)
            .RuleFor(r => r.FullName, f => f.Name.FullName())
            .RuleFor(r => r.Address, f => f.Address.FullAddress())
            .RuleFor(r => r.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(r => r.RegistrationDate, f => f.Date.Past(5));

        return faker.Generate(count);
    }

    /// <summary>
    /// Generates a list of issue records (book borrowings).
    /// </summary>
    /// <param name="books">List of available books.</param>
    /// <param name="readers">List of readers who borrow books.</param>
    /// <param name="count">Number of issues to generate (default: 10).</param>
    /// <returns>List of generated <see cref="Issue"/> objects.</returns>
    public static List<Issue> GenerateIssues(List<Book> books, List<Reader> readers, int count = 10)
    {
        var faker = new Faker<Issue>()
            .RuleFor(i => i.Id, f => f.IndexFaker + 1)
            .RuleFor(i => i.BookId, f => f.PickRandom(books).Id)
            .RuleFor(i => i.ReaderId, f => f.PickRandom(readers).Id)
            .RuleFor(i => i.IssueDate, f => f.Date.Past(1))
            .RuleFor(i => i.DaysCount, f => f.Random.Int(7, 30));

        return faker.Generate(count);
    }
}
