using AutoMapper;
using Library.Application.Contracts.BookTypes;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над типами книг.
/// Реализует интерфейс IBookTypeService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторию BookTypeRepository.
/// </summary>
public class BookTypeService : IBookTypeService
{
    private readonly BookTypeRepository _bookTypeRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует сервис с репозиторием и маппером.
    /// </summary>
    /// <param name="bookTypeRepository">Репозиторий для работы с типами книг в БД</param>
    /// <param name="mapper">AutoMapper для преобразования сущностей в DTO и обратно</param>
    public BookTypeService(BookTypeRepository bookTypeRepository, IMapper mapper)
    {
        _bookTypeRepository = bookTypeRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить тип книги по идентификатору.
    /// Загружает тип с репозитория и преобразует в DTO через AutoMapper.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>BookTypeDto или null если тип не найден</returns>
    public async Task<BookTypeDto?> GetAsync(int id)
    {
        var bookType = await _bookTypeRepository.ReadAsync(id);
        return bookType == null ? null : _mapper.Map<BookTypeDto>(bookType);
    }

    /// <summary>
    /// Получить список всех типов книг.
    /// Загружает все типы с репозитория и преобразует в список DTO.
    /// </summary>
    /// <returns>Неизменяемый список BookTypeDto всех типов книг</returns>
    public async Task<IReadOnlyList<BookTypeDto>> GetListAsync()
    {
        var bookTypes = await _bookTypeRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<BookTypeDto>>(bookTypes);
    }

    /// <summary>
    /// Создать новый тип книги.
    /// Преобразует DTO в сущность, сохраняет в БД и возвращает созданный DTO.
    /// ID должен быть установлен вручную в DTO.
    /// </summary>
    /// <param name="input">DTO с данными нового типа (Name и Id обязательны)</param>
    /// <returns>BookTypeDto созданного типа с заполненным Id</returns>
    /// <exception cref="ArgumentException">Выбрасывается если ID не установлен или <= 0</exception>
    public async Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input)
    {
        if (input.Id <= 0)
            throw new ArgumentException("ID должен быть установлен вручную и быть больше 0", nameof(input.Id));

        var bookType = _mapper.Map<BookType>(input);
        var created = await _bookTypeRepository.CreateAsync(bookType);
        return _mapper.Map<BookTypeDto>(created);
    }

    /// <summary>
    /// Обновить существующий тип книги.
    /// Загружает текущий тип, применяет изменения, сохраняет в БД.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа для обновления</param>
    /// <param name="input">DTO с новыми данными типа</param>
    /// <returns>BookTypeDto обновленного типа или null если тип не найден</returns>
    public async Task<BookTypeDto?> UpdateAsync(int id, BookTypeCreateUpdateDto input)
    {
        var existing = await _bookTypeRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _bookTypeRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<BookTypeDto>(updated);
    }

    /// <summary>
    /// Удалить тип книги по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await _bookTypeRepository.DeleteAsync(id);
    }
}
