namespace Library.Domain.Models;

// Book issue record (when a reader takes a book from the library)
public class Issue
{
    public int Id { get; set; }                 // Primary key
    public int BookId { get; set; }             // Reference to the book
    public int ReaderId { get; set; }           // Reference to the reader
    public DateTime IssueDate { get; set; }     // Date when the book was issued
    public int DaysCount { get; set; }          // For how many days the book was given
}