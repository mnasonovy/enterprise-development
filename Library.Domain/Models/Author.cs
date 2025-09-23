namespace Library.Domain.Models;

// Author reference
public class Author
{
    public int Id { get; set; }              // Primary key
    public string Initials { get; set; } = ""; // Initials (e.g., "L.N.")
    public string LastName { get; set; } = ""; // Last name (e.g., "Tolstoy")
}