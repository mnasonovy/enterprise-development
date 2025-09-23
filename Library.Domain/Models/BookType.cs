namespace Library.Domain.Models;

// Book type reference (e.g., novel, textbook, magazine)
public class BookType
{
    public int Id { get; set; }                // Primary key
    public string Name { get; set; } = "";     // Type name (e.g., "Novel", "Textbook")
}   