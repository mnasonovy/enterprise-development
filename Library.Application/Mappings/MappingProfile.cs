using AutoMapper;

using Library.Application.Contracts.Analytics;
using Library.Application.Contracts.Authors;
using Library.Application.Contracts.Books;
using Library.Application.Contracts.BookTypes;
using Library.Application.Contracts.Issues;
using Library.Application.Contracts.Publishers;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;

namespace Library.Application.Mappings;

/// <summary>
/// AutoMapper профиль для конфигурации всех маппингов между DTO и Domain моделями.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        MapAuthors();
        MapBooks();
        MapBookTypes();
        MapIssues();
        MapPublishers();
        MapReaders();
        MapAnalytics();
    }

    /// <summary>Маппинги для авторов</summary>
    private void MapAuthors()
    {
        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<AuthorCreateUpdateDto, Author>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }

    /// <summary>Маппинги для книг</summary>
    private void MapBooks()
    {
        CreateMap<Book, BookDto>()
            .ForMember(d => d.BookTypeName,
                opt => opt.MapFrom(s => s.BookType.Name))
            .ForMember(d => d.PublisherName,
                opt => opt.MapFrom(s => s.Publisher.Name))
            .ForMember(d => d.AuthorNames,
                opt => opt.MapFrom(s => s.Authors.Select(a => a.LastName)));

        CreateMap<Book, BookCreateUpdateDto>().ReverseMap();
    }

    /// <summary>Маппинги для типов книг</summary>
    private void MapBookTypes()
    {
        CreateMap<BookType, BookTypeDto>().ReverseMap();
        CreateMap<BookType, BookTypeCreateUpdateDto>().ReverseMap();
    }

    /// <summary>Маппинги для выданных книг</summary>
    private void MapIssues()
    {
        CreateMap<Issue, IssueDto>()
            .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.ReaderFullName,
                opt => opt.MapFrom(src => src.Reader.FullName));

        CreateMap<Issue, IssueCreateUpdateDto>().ReverseMap();
    }

    /// <summary>Маппинги для издателей</summary>
    private void MapPublishers()
    {
        CreateMap<Publisher, PublisherDto>().ReverseMap();
        CreateMap<Publisher, PublisherCreateUpdateDto>().ReverseMap();
    }

    /// <summary>Маппинги для читателей</summary>
    private void MapReaders()
    {
        CreateMap<Reader, ReaderDto>().ReverseMap();
        CreateMap<Reader, ReaderCreateUpdateDto>().ReverseMap();
    }

    /// <summary>Маппинги для аналитических DTO</summary>
    private void MapAnalytics()
    {
        CreateMap<Reader, TopReaderDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName));

        CreateMap<Reader, ReaderDaysCountDto>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName));

        CreateMap<Publisher, TopPublisherDto>()
            .ForMember(dest => dest.PublisherName,
                opt => opt.MapFrom(src => src.Name));

        CreateMap<Book, TopBookDto>()
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.TimesIssued,
                opt => opt.Ignore());
    }
}
