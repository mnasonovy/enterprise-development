using Bogus;
using Library.Domain.Models;

namespace Library.Domain.Data;

// Generates test data for the library system
public static class DataSeeder
{
    public static List<Author> GenerateAuthors(int count = 10)
    {
        var faker = new Faker<Author>()
            .RuleFor(a => a.Id, f => f.IndexFaker + 1)
            .RuleFor(a => a.Initials, f => f.Name.FirstName()[0] + ".")
            .RuleFor(a => a.LastName, f => f.Name.LastName());

        return faker.Generate(count);
    }

    public static List<Publisher> GeneratePublishers(int count = 10)
    {
        var faker = new Faker<Publisher>()
            .RuleFor(p => p.Id, f => f.IndexFaker + 1)
            .RuleFor(p => p.Name, f => f.Company.CompanyName());

        return faker.Generate(count);
    }

    public static List<BookType> GenerateBookTypes(int count = 5)
    {
        var faker = new Faker<BookType>()
            .RuleFor(bt => bt.Id, f => f.IndexFaker + 1)
            .RuleFor(bt => bt.Name, f => f.Commerce.Categories(1)[0]);

        return faker.Generate(count);
    }
}