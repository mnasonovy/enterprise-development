namespace Library.Domain.Models;

// Book catalog entry
public class Book
{
    public int Id { get; set; }                        // Primary key
    public string AlphabetCode { get; set; } = "";     // Alphabet catalog code
    public string Title { get; set; } = "";            // Book title
    public int Year { get; set; }                      // Publication year

    // References
    public int BookTypeId { get; set; }                // Reference to BookType
    public int PublisherId { get; set; }               // Reference to Publisher

    // Many-to-many relation: one book can have multiple authors
    public List<int> AuthorIds { get; set; } = [];
}