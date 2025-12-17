using AutoMapper;
using Library.Application.Contracts.Analytics;
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
/// 
/// Назначение:
/// - Определяет двусторонние маппинги для всех сущностей системы
/// - Преобразует Domain модели в DTO для API-ответов
/// - Преобразует CreateUpdateDto в Domain модели для создания/обновления
/// - Конфигурирует пользовательские преобразования для сложных полей
/// 
/// Поддерживаемые сущности:
/// - Author (авторы)
/// - Book (книги)
/// - BookType (типы книг)
/// - Issue (выданные книги)
/// - Publisher (издатели)
/// - Reader (читатели)
/// - Analytics DTO (аналитические данные)
/// 
/// Маппинги включают:
/// - Двусторонние маппинги (ReverseMap) для симметричных преобразований
/// - Пользовательские трансформации (ForMember) для сложных полей
/// - Включенные навигационные свойства для загруженных связанных сущностей
/// 
/// Использование:
/// - Автоматически регистрируется в DI контейнере через AddAutoMapper()
/// - Используется в сервисах для преобразования данных
/// - Упрощает кодовую базу и снижает количество ручного маппинга
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Инициализирует AutoMapper профиль со всеми маппингами между Domain моделями и DTO.
    /// 
    /// Процесс инициализации:
    /// 1. Регистрирует двусторонние маппинги для основных сущностей
    /// 2. Конфигурирует пользовательские преобразования для вычисляемых полей
    /// 3. Загружает связанные сущности для полной информации в ответах
    /// 
    /// Вызывается автоматически при первом использовании AutoMapper.
    /// </summary>
    public MappingProfile()
    {
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // AUTHOR Mappings - Маппинги для авторов
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Author <-> AuthorDto: двусторонний маппинг
        // Преобразует Domain модель Author в DTO для API-ответов и наоборот
        CreateMap<Author, AuthorDto>().ReverseMap();

        // Author <-> AuthorCreateUpdateDto: двусторонний маппинг
        // Используется при создании и обновлении авторов через API
        CreateMap<Author, AuthorCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BOOK Mappings - Маппинги для книг
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Book <-> BookDto: двусторонний маппинг
        // Преобразует Domain модель Book (с навигационными свойствами) в DTO
        // Включает информацию об авторах, типе книги и издателе
        CreateMap<Book, BookDto>().ReverseMap();

        // Book <-> BookCreateUpdateDto: двусторонний маппинг
        // Используется при создании и обновлении книг через API
        // Требует BookTitle, AuthorId, BookTypeId, PublisherId
        CreateMap<Book, BookCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BOOKTYPE Mappings - Маппинги для типов книг
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // BookType <-> BookTypeDto: двусторонний маппинг
        // Преобразует Domain модель BookType в DTO для API-ответов
        // Примеры: Роман, Научная литература, Справочник, Энциклопедия
        CreateMap<BookType, BookTypeDto>().ReverseMap();

        // BookType <-> BookTypeCreateUpdateDto: двусторонний маппинг
        // Используется при создании и обновлении типов книг через API
        CreateMap<BookType, BookTypeCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // ISSUE Mappings - Маппинги для выданных книг
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Issue -> IssueDto: маппинг с пользовательскими трансформациями
        // Загружает информацию о книге и читателе через навигационные свойства
        // Преобразует связанные сущности в строковые поля для DTO
        CreateMap<Issue, IssueDto>()
            // BookTitle: извлекает название из связанной сущности Book
            // Используется MongoDB EF Core Entry().LoadAsync() для загрузки Book перед маппингом
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            
            // ReaderFullName: извлекает полное имя из связанной сущности Reader
            // Используется MongoDB EF Core Entry().LoadAsync() для загрузки Reader перед маппингом
            .ForMember(dest => dest.ReaderFullName, opt => opt.MapFrom(src => src.Reader.FullName));

        // Issue <-> IssueCreateUpdateDto: двусторонний маппинг
        // Используется при создании и обновлении выданных книг через API
        // Требует BookId, ReaderId, IssueDate, DaysCount
        // ReturnDate опционально для отметки возврата книги
        CreateMap<Issue, IssueCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // PUBLISHER Mappings - Маппинги для издателей
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Publisher <-> PublisherDto: двусторонний маппинг
        // Преобразует Domain модель Publisher в DTO для API-ответов
        // Содержит информацию об издательстве и его книгах
        CreateMap<Publisher, PublisherDto>().ReverseMap();

        // Publisher <-> PublisherCreateUpdateDto: двусторонний маппинг
        // Используется при создании и обновлении издателей через API
        CreateMap<Publisher, PublisherCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // READER Mappings - Маппинги для читателей
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // Reader <-> ReaderDto: двусторонний маппинг
        // Преобразует Domain модель Reader в DTO для API-ответов
        // Содержит полное имя, email и другую информацию о читателе
        CreateMap<Reader, ReaderDto>().ReverseMap();

        // Reader <-> ReaderCreateUpdateDto: двусторонний маппинг
        // Используется при создании и обновлении читателей через API
        CreateMap<Reader, ReaderCreateUpdateDto>().ReverseMap();

        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // ANALYTICS DTO Mappings - Маппинги для аналитических данных
        // ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
        // TopReaderDto: DTO для топ читателей
        // Содержит полное имя (FullName) и количество взятых книг (CountBooks)
        // Используется в аналитических отчетах для выявления активных читателей
        CreateMap<Reader, TopReaderDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        // ReaderDaysCountDto: DTO для статистики читателей по дням
        // Содержит полное имя (FullName) и общее количество дней (CountDays)
        // Используется для анализа средней длительности использования книг
        CreateMap<Reader, ReaderDaysCountDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

        // TopPublisherDto: DTO для топ издательств
        // Содержит название издательства (PublisherName) и количество выданных книг (CountBooks)
        // Используется для анализа популярности издательств за период
        CreateMap<Publisher, TopPublisherDto>()
            .ForMember(dest => dest.PublisherName, opt => opt.MapFrom(src => src.Name));

        // TopBookDto: DTO для популярных и непопулярных книг
        // Содержит название книги (Title) и количество выданий (TimesIssued)
        // Используется для выявления как популярных, так и невостребованной литературы
        CreateMap<Book, TopBookDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.TimesIssued, opt => opt.Ignore()); // Вычисляется в сервисе
    }
}
