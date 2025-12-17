using AutoMapper;
using Library.Application.Contracts.Books;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления книгами в библиотеке.
/// Реализует бизнес-логику работы с книгами, включая CRUD операции,
/// управление авторами и маппирование данных в DTO.
/// </summary>
public class BookService : IBookService
{
    private readonly BookRepository _bookRepository;
    private readonly BookTypeRepository _bookTypeRepository;
    private readonly PublisherRepository _publisherRepository;
    private readonly AuthorRepository _authorRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="BookService"/>.
    /// </summary>
    /// <param name="bookRepository">Репозиторий для работы с книгами</param>
    /// <param name="bookTypeRepository">Репозиторий для работы с типами книг</param>
    /// <param name="publisherRepository">Репозиторий для работы с издателями</param>
    /// <param name="authorRepository">Репозиторий для работы с авторами</param>
    /// <param name="mapper">AutoMapper для преобразования объектов</param>
    public BookService(
        BookRepository bookRepository,
        BookTypeRepository bookTypeRepository,
        PublisherRepository publisherRepository,
        AuthorRepository authorRepository,
        IMapper mapper)
    {
        _bookRepository = bookRepository;
        _bookTypeRepository = bookTypeRepository;
        _publisherRepository = publisherRepository;
        _authorRepository = authorRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Преобразует объект книги в DTO с дополнительной информацией.
    /// Загружает имена типа книги, издателя и форматирует имена авторов 
    /// в формат "Инициалы Фамилия".
    /// </summary>
    /// <param name="book">Объект книги из БД с загруженными авторами</param>
    /// <returns>DTO книги с полной информацией для клиента</returns>
    private async Task<BookDto> MapToBookDtoAsync(Book book)
    {
        var bookDto = _mapper.Map<BookDto>(book);

        if (book.BookTypeId > 0)
        {
            var bookType = await _bookTypeRepository.ReadAsync(book.BookTypeId);
            bookDto.BookTypeName = bookType?.Name ?? "Unknown";
        }

        if (book.PublisherId > 0)
        {
            var publisher = await _publisherRepository.ReadAsync(book.PublisherId);
            bookDto.PublisherName = publisher?.Name ?? "Unknown";
        }

        if (book.Authors is not null && book.Authors.Count > 0)
        {
            var authorNames = new List<string>();
            foreach (var author in book.Authors)
            {
                var fullName = string.IsNullOrWhiteSpace(author.Initials)
                    ? author.LastName
                    : $"{author.Initials} {author.LastName}";
                authorNames.Add(fullName);
            }

            bookDto.AuthorNames = authorNames;
        }

        return bookDto;
    }

    /// <summary>
    /// Получает книгу по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>DTO книги с полной информацией или <c>null</c> если не найдена</returns>
    public async Task<BookDto?> GetAsync(int id)
    {
        var book = await _bookRepository.ReadAsync(id);
        return book is null ? null : await MapToBookDtoAsync(book);
    }

    /// <summary>
    /// Получает список всех книг из каталога.
    /// </summary>
    /// <returns>Доступная только для чтения коллекция DTO всех книг</returns>
    public async Task<IReadOnlyList<BookDto>> GetListAsync()
    {
        var books = await _bookRepository.ReadAllAsync();
        var bookDtos = new List<BookDto>();
        foreach (var book in books)
        {
            bookDtos.Add(await MapToBookDtoAsync(book));
        }

        return bookDtos.AsReadOnly();
    }

    /// <summary>
    /// Создает новую книгу в каталоге библиотеки.
    /// Валидирует входные данные, загружает авторов по их идентификаторам
    /// и сохраняет только ID авторов в коллекцию AuthorIds.
    /// </summary>
    /// <param name="input">DTO для создания книги с данными и ID авторов</param>
    /// <returns>DTO созданной книги с полной информацией</returns>
    /// <exception cref="ArgumentException">Если ID книги меньше или равен 0</exception>
    public async Task<BookDto> CreateAsync(BookCreateUpdateDto input)
    {
        if (input.Id <= 0)
            throw new ArgumentException("ID должен быть установлен вручную и быть больше 0", nameof(input.Id));

        var book = _mapper.Map<Book>(input);

        if (input.AuthorIds is not null && input.AuthorIds.Count > 0)
        {
            var authors = new List<Author>();
            foreach (var authorId in input.AuthorIds)
            {
                var author = await _authorRepository.ReadAsync(authorId);
                if (author is not null)
                {
                    authors.Add(author);
                }
            }

            book.AuthorIds = authors.Select(a => a.Id).ToList();
            book.Authors = [];
        }

        var created = await _bookRepository.CreateAsync(book);
        return await MapToBookDtoAsync(created);
    }

    /// <summary>
    /// Обновляет существующую книгу в каталоге.
    /// Загружает авторов по их идентификаторам и сохраняет только ID авторов.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для обновления</param>
    /// <param name="input">DTO с новыми данными книги и ID авторов</param>
    /// <returns>DTO обновленной книги с полной информацией или <c>null</c> если не найдена</returns>
    public async Task<BookDto?> UpdateAsync(int id, BookCreateUpdateDto input)
    {
        var existing = await _bookRepository.ReadAsync(id);
        if (existing is null)
            return null;

        _mapper.Map(input, existing);

        if (input.AuthorIds is not null && input.AuthorIds.Count > 0)
        {
            var authors = new List<Author>();
            foreach (var authorId in input.AuthorIds)
            {
                var author = await _authorRepository.ReadAsync(authorId);
                if (author is not null)
                {
                    authors.Add(author);
                }
            }

            existing.AuthorIds = authors.Select(a => a.Id).ToList();
            existing.Authors = [];
        }
        else
        {
            existing.AuthorIds.Clear();
            existing.Authors.Clear();
        }

        var updated = await _bookRepository.UpdateAsync(existing);
        return updated is null ? null : await MapToBookDtoAsync(updated);
    }

    /// <summary>
    /// Удаляет книгу из каталога по идентификатору.
    /// При удалении также удаляются все связанные выпуски (экземпляры) книги.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await _bookRepository.DeleteAsync(id);
    }
}
