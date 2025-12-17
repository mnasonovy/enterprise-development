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
/// AutoMapper профиль для конфигурации всех маппингов DTO ↔ Entity
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Author Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        CreateMap<AuthorDto, Author>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Book Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        CreateMap<BookDto, Book>().ReverseMap();
        CreateMap<BookCreateUpdateDto, Book>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BookType Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        CreateMap<BookTypeDto, BookType>().ReverseMap();
        CreateMap<BookTypeCreateUpdateDto, BookType>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Issue Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        CreateMap<IssueDto, Issue>().ReverseMap();
        CreateMap<IssueCreateUpdateDto, Issue>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Publisher Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        CreateMap<PublisherDto, Publisher>().ReverseMap();
        CreateMap<PublisherCreateUpdateDto, Publisher>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Reader Mappings
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        CreateMap<ReaderDto, Reader>().ReverseMap();
        CreateMap<ReaderCreateUpdateDto, Reader>().ReverseMap();
    }
}
