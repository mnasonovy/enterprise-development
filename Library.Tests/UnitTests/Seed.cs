using Library.Domain.Models;

namespace Library.Tests.UnitTests;

/// <summary>
/// Seeds test data for the library system: authors, publishers, book types, books, readers, and issues.
/// Includes diverse scenarios and edge cases for robust unit testing.
/// </summary>
public class DataSeed
{
    private readonly List<Author> _authors;
    private readonly List<Publisher> _publishers;
    private readonly List<BookType> _bookTypes;
    private readonly List<Book> _books;
    private readonly List<Reader> _readers;
    private readonly List<Issue> _issues;

    public DataSeed()
    {
        _authors = InitAuthors();
        _publishers = InitPublishers();
        _bookTypes = InitBookTypes();
        _books = InitBooks();
        _readers = InitReaders();
        _issues = InitIssues();
    }

    public List<Author> Authors => _authors;
    public List<Publisher> Publishers => _publishers;
    public List<BookType> BookTypes => _bookTypes;
    public List<Book> Books => _books;
    public List<Reader> Readers => _readers;
    public List<Issue> Issues => _issues;

    private List<Author> InitAuthors() =>
    [
        new Author { Id = 1,  Initials = "L.N.", LastName = "Tolstoy" },
        new Author { Id = 2,  Initials = "F.M.", LastName = "Dostoevsky" },
        new Author { Id = 3,  Initials = "A.S.", LastName = "Pushkin" },
        new Author { Id = 4,  Initials = "M.A.", LastName = "Bulgakov" },
        new Author { Id = 5,  Initials = "A.P.", LastName = "Chekhov" },
        new Author { Id = 6,  Initials = "N.V.", LastName = "Gogol" },
        new Author { Id = 7,  Initials = "I.S.", LastName = "Turgenev" },
        new Author { Id = 8,  Initials = "B.L.", LastName = "Pasternak" },
        new Author { Id = 9,  Initials = "M.",   LastName = "Gorky" },
        new Author { Id = 10, Initials = "V.V.", LastName = "Nabokov" },
    ];

    private List<Publisher> InitPublishers() =>
    [
        new Publisher { Id = 1,  Name = "Eksmo" },
        new Publisher { Id = 2,  Name = "AST" },
        new Publisher { Id = 3,  Name = "Piter" },
        new Publisher { Id = 4,  Name = "Drofa" },
        new Publisher { Id = 5,  Name = "MIF" },
        new Publisher { Id = 6,  Name = "Prosveshchenie" },
        new Publisher { Id = 7,  Name = "Veche" },
        new Publisher { Id = 8,  Name = "Azbuka" },
        new Publisher { Id = 9,  Name = "Alpina" },
        new Publisher { Id = 10, Name = "Phoenix" },
    ];

    private List<BookType> InitBookTypes() =>
    [
        new BookType { Id = 1,  Name = "Novel" },
        new BookType { Id = 2,  Name = "Short Stories" },
        new BookType { Id = 3,  Name = "Drama" },
        new BookType { Id = 4,  Name = "Poetry" },
        new BookType { Id = 5,  Name = "Textbook" },
        new BookType { Id = 6,  Name = "Magazine" },
        new BookType { Id = 7,  Name = "Reference" },
        new BookType { Id = 8,  Name = "Non-fiction" },
        new BookType { Id = 9,  Name = "Children" },
        new BookType { Id = 10, Name = "Fantasy" },
    ];

    private List<Book> InitBooks() =>
    [
        new Book { Id = 1,  Title = "Ancient Philosophy", Year = 1800, BookType = _bookTypes[7], Publisher = _publishers[0], Authors = [_authors[2]] },
        new Book { Id = 2,  Title = "Modern Programming", Year = 2025, BookType = _bookTypes[4], Publisher = _publishers[1], Authors = [_authors[1]] },
        new Book { Id = 3,  Title = "War and Peace", Year = 1869, AlphabetCode = "WNP", BookType = _bookTypes[0], Publisher = _publishers[2], Authors = [_authors[0]] },
        new Book { Id = 4,  Title = "Crime and Punishment", Year = 1866, AlphabetCode = "CAP", BookType = _bookTypes[0], Publisher = _publishers[3], Authors = [_authors[1]] },
        new Book { Id = 5,  Title = "Eugene Onegin", Year = 1833, AlphabetCode = "EON", BookType = _bookTypes[3], Publisher = _publishers[4], Authors = [_authors[2]] },
        new Book { Id = 6,  Title = "The Master and Margarita", Year = 1967, AlphabetCode = "MAM", BookType = _bookTypes[0], Publisher = _publishers[5], Authors = [_authors[3]] },
        new Book { Id = 7,  Title = "Collected Works", Year = 1900, BookType = _bookTypes[1], Publisher = _publishers[6], Authors = [_authors[4], _authors[5]] },
        new Book { Id = 8,  Title = "Doctor Zhivago", Year = 1957, AlphabetCode = "DZ", BookType = _bookTypes[0], Publisher = _publishers[7], Authors = [_authors[7]] },
        new Book { Id = 9,  Title = "Mother", Year = 1906, AlphabetCode = "MOT", BookType = _bookTypes[0], Publisher = _publishers[8], Authors = [_authors[8]] },
        new Book { Id = 10, Title = "Lolita", Year = 1955, AlphabetCode = "LOL", BookType = _bookTypes[0], Publisher = _publishers[9], Authors = [_authors[9]] },
        new Book { Id = 11, Title = "A Month in the Country", Year = 1855, AlphabetCode = "AMC", BookType = _bookTypes[2], Publisher = _publishers[0], Authors = [_authors[6]] },
        new Book { Id = 12, Title = "Modern Review", Year = 2023, AlphabetCode = "MR", BookType = _bookTypes[5], Publisher = _publishers[1], Authors = [_authors[0], _authors[1], _authors[2]] },
        new Book { Id = 13, Title = "Literary Encyclopedia", Year = 1990, AlphabetCode = "LEN", BookType = _bookTypes[6], Publisher = _publishers[2], Authors = [_authors[4]] },
        new Book { Id = 14, Title = "Children's Tales", Year = 1875, AlphabetCode = "CTA", BookType = _bookTypes[8], Publisher = _publishers[3], Authors = [_authors[0]] },
        new Book { Id = 15, Title = "The Enchanted Forest", Year = 2010, AlphabetCode = "TEF", BookType = _bookTypes[9], Publisher = _publishers[4], Authors = [_authors[9]] }
    ];

    private List<Reader> InitReaders() =>
    [
        new Reader { Id = 1, FullName = "Ivan Petrov", Address = "Moscow", Phone = "12345", RegistrationDate = new DateTime(2020, 1, 1) },
        new Reader { Id = 2, FullName = "Anna Ivanova", Address = "SPb", Phone = "67890", RegistrationDate = new DateTime(2021, 2, 1) },
        new Reader { Id = 3, FullName = "Sergey Smirnov", Address = "", Phone = "11111", RegistrationDate = new DateTime(2022, 3, 1) },
        new Reader { Id = 4, FullName = "Olga Sokolova", Address = "Perm", Phone = null!, RegistrationDate = new DateTime(2020, 4, 1) },
        new Reader { Id = 5, FullName = "Pavel Kuznetsov", Address = "Samara", Phone = "33333", RegistrationDate = DateTime.Today },
        new Reader { Id = 6, FullName = "Elena Popova", Address = "Moscow", Phone = "44444", RegistrationDate = DateTime.Today.AddYears(-20) },
        new Reader { Id = 7, FullName = "Dmitry Volkov", Address = "Tula", Phone = "55555", RegistrationDate = new DateTime(2022, 7, 1) },
        new Reader { Id = 8, FullName = "Maria Morozova", Address = "Sochi", Phone = "66666", RegistrationDate = new DateTime(2021, 8, 1) },
        new Reader { Id = 9, FullName = "Nikita Lebedev", Address = "Minsk", Phone = "77777", RegistrationDate = new DateTime(2020, 9, 1) },
        new Reader { Id = 10, FullName = "Kseniya Vlasova", Address = "Rostov", Phone = "88888", RegistrationDate = new DateTime(2023, 10, 1) },
        new Reader { Id = 11, FullName = "Alexey Mikhailov", Address = "Novosibirsk", Phone = "99999", RegistrationDate = new DateTime(2024, 11, 11) },
        new Reader { Id = 12, FullName = "Irina Sidorova", Address = "London", Phone = "22222", RegistrationDate = new DateTime(2019, 12, 31) }
    ];

    private List<Issue> InitIssues() =>
    [
        new Issue { Id = 1, Book = _books[0], Reader = _readers[0], IssueDate = new DateTime(2025, 1, 10), DaysCount = 14 },
        new Issue { Id = 2, Book = _books[1], Reader = _readers[1], IssueDate = new DateTime(2025, 2, 5), DaysCount = 21 },
        new Issue { Id = 3, Book = _books[2], Reader = _readers[2], IssueDate = DateTime.Today, DaysCount = 1 },
        new Issue { Id = 4, Book = _books[3], Reader = _readers[3], IssueDate = DateTime.Today.AddDays(-400), DaysCount = 365 },
        new Issue { Id = 5, Book = _books[4], Reader = _readers[4], IssueDate = DateTime.Today.AddDays(5), DaysCount = 10 },
        new Issue { Id = 6, Book = _books[0], Reader = _readers[5], IssueDate = new DateTime(2025, 3, 15), DaysCount = 7 },
        new Issue { Id = 7, Book = _books[0], Reader = _readers[6], IssueDate = new DateTime(2025, 4, 1), DaysCount = 10 },
        new Issue { Id = 8, Book = _books[5], Reader = _readers[0], IssueDate = new DateTime(2025, 5, 2), DaysCount = 14 },
        new Issue { Id = 9, Book = _books[6], Reader = _readers[0], IssueDate = new DateTime(2025, 6, 2), DaysCount = 14 },
        new Issue { Id = 10, Book = _books[7], Reader = _readers[0], IssueDate = new DateTime(2025, 7, 2), DaysCount = 14 },
        new Issue { Id = 11, Book = _books[12], Reader = _readers[1], IssueDate = new DateTime(2025, 8, 1), DaysCount = 2 },
        new Issue { Id = 12, Book = _books[10], Reader = _readers[2], IssueDate = new DateTime(2025, 8, 10), DaysCount = 14 },
        new Issue { Id = 13, Book = _books[11], Reader = _readers[5], IssueDate = new DateTime(2025, 9, 1), DaysCount = 3 },
        new Issue { Id = 14, Book = _books[13], Reader = _readers[6], IssueDate = new DateTime(2025, 9, 10), DaysCount = 10 },
        new Issue { Id = 15, Book = _books[14], Reader = _readers[10], IssueDate = new DateTime(2025, 10, 1), DaysCount = 14 },
        new Issue { Id = 16, Book = _books[11], Reader = _readers[10], IssueDate = new DateTime(2025, 10, 15), DaysCount = 3 },
        new Issue { Id = 17, Book = _books[13], Reader = _readers[11], IssueDate = new DateTime(2025, 11, 1), DaysCount = 14 },
        new Issue { Id = 18, Book = _books[14], Reader = _readers[11], IssueDate = new DateTime(2025, 11, 10), DaysCount = 21 },
        new Issue { Id = 19, Book = _books[2], Reader = _readers[1], IssueDate = new DateTime(2025, 4, 15), DaysCount = 30 },
        new Issue { Id = 20, Book = _books[5], Reader = _readers[2], IssueDate = new DateTime(2025, 6, 1), DaysCount = 14 }
    ];
}
