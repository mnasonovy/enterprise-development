using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using System.Text.Json.Serialization;

namespace LibraryClient.Models;

public class BookClientDto
{
    public int Id { get; set; }
    public string? AlphabetCode { get; set; }
    public required string Title { get; set; }
    public int Year { get; set; }
    public int BookTypeId { get; set; }

    [JsonPropertyName("bookType")]
    public BookTypeDto? BookType { get; set; }  // ✅ Сделали nullable

    public int PublisherId { get; set; }

    [JsonPropertyName("publisher")]
    public PublisherDto? Publisher { get; set; }  // ✅ Сделали nullable

    public List<int> AuthorIds { get; set; } = [];
    public List<AuthorClientDto> Authors { get; set; } = [];
    public List<IssueDto> Issues { get; set; } = [];
}
