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
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // AUTHOR Mappings - Маппинги для авторов
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Author, AuthorDto>().ReverseMap();
        CreateMap<Author, AuthorCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BOOK Mappings - Маппинги для книг
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        // Book -> BookDto: берём имена из навигаций, которые грузит репозиторий
        CreateMap<Book, BookDto>()
            .ForMember(d => d.BookTypeName,
                opt => opt.MapFrom(s => s.BookType.Name))
            .ForMember(d => d.PublisherName,
                opt => opt.MapFrom(s => s.Publisher.Name))
            .ForMember(d => d.AuthorNames,
                opt => opt.MapFrom(s => s.Authors.Select(a => a.LastName)));

        // Book <-> BookCreateUpdateDto: двусторонний маппинг для создания/обновления
        CreateMap<Book, BookCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BOOKTYPE Mappings - Маппинги для типов книг
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<BookType, BookTypeDto>().ReverseMap();
        CreateMap<BookType, BookTypeCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // ISSUE Mappings - Маппинги для выданных книг
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Issue, IssueDto>()
            .ForMember(dest => dest.BookTitle,
                opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.ReaderFullName,
                opt => opt.MapFrom(src => src.Reader.FullName));

        CreateMap<Issue, IssueCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // PUBLISHER Mappings - Маппинги для издателей
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Publisher, PublisherDto>().ReverseMap();
        CreateMap<Publisher, PublisherCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // READER Mappings - Маппинги для читателей
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

        CreateMap<Reader, ReaderDto>().ReverseMap();
        CreateMap<Reader, ReaderCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // ANALYTICS DTO Mappings - Маппинги для аналитики
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━

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
                opt => opt.Ignore()); // вычисляется в сервисе
    }
}
