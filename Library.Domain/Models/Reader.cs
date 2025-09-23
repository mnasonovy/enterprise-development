namespace Library.Domain.Models;

// Library reader card
public class Reader
{
    public int Id { get; set; }                   // Primary key
    public string FullName { get; set; } = "";    // Full name
    public string Address { get; set; } = "";     // Address
    public string Phone { get; set; } = "";       // Phone number
    public DateTime RegistrationDate { get; set; } // Registration date
}