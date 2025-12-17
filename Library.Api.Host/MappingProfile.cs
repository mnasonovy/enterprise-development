using AutoMapper;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;

namespace Library.Api.Host;

/// <summary>
/// AutoMapper профиль для конфигурации всех маппингов между DTO и Domain моделями.
/// Определяет двусторонние маппинги для всех сущностей системы.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Author Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Author, AuthorDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Book Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Book, BookDto>().ReverseMap();
        CreateMap<Book, BookCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BookType Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<BookType, BookTypeDto>().ReverseMap();
        CreateMap<BookType, BookTypeCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Issue Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Issue, IssueDto>().ReverseMap();
        CreateMap<Issue, IssueCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Publisher Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Publisher, PublisherDto>().ReverseMap();
        CreateMap<Publisher, PublisherCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Reader Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Reader, ReaderDto>().ReverseMap();
        CreateMap<Reader, ReaderCreateUpdateDto>().ReverseMap();
    }
}
