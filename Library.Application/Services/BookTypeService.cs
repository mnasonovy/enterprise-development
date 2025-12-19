using AutoMapper;
using Library.Application.Contracts.BookTypes;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Contracts;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над типами книг.
/// Реализует интерфейс IBookTypeService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторию через интерфейс.
/// </summary>
public class BookTypeService(IBookTypeRepository bookTypeRepository, IMapper mapper) : IBookTypeService
{
    /// <summary>
    /// Получить тип книги по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа книги</param>
    /// <returns>BookTypeDto или null если тип не найден</returns>
    public async Task<BookTypeDto?> GetAsync(int id)
    {
        var bookType = await bookTypeRepository.ReadAsync(id);
        return bookType == null ? null : mapper.Map<BookTypeDto>(bookType);
    }

    /// <summary>
    /// Получить список всех типов книг.
    /// </summary>
    /// <returns>Неизменяемый список BookTypeDto всех типов книг</returns>
    public async Task<IReadOnlyList<BookTypeDto>> GetListAsync()
    {
        var bookTypes = await bookTypeRepository.ReadAllAsync();
        return mapper.Map<IReadOnlyList<BookTypeDto>>(bookTypes);
    }

    /// <summary>
    /// Создать новый тип книги.
    /// </summary>
    /// <param name="input">DTO с данными нового типа</param>
    /// <returns>BookTypeDto созданного типа с заполненным Id</returns>
    public async Task<BookTypeDto> CreateAsync(BookTypeCreateUpdateDto input)
    {
        var bookType = mapper.Map<BookType>(input);
        var created = await bookTypeRepository.CreateAsync(bookType);
        return mapper.Map<BookTypeDto>(created);
    }

    /// <summary>
    /// Обновить существующий тип книги.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа для обновления</param>
    /// <param name="input">DTO с новыми данными типа</param>
    /// <returns>BookTypeDto обновленного типа или null если тип не найден</returns>
    public async Task<BookTypeDto?> UpdateAsync(int id, BookTypeCreateUpdateDto input)
    {
        var existing = await bookTypeRepository.ReadAsync(id);
        if (existing == null)
            return null;

        mapper.Map(input, existing);
        var updated = await bookTypeRepository.UpdateAsync(existing);
        return updated == null ? null : mapper.Map<BookTypeDto>(updated);
    }

    /// <summary>
    /// Удалить тип книги по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор типа для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await bookTypeRepository.DeleteAsync(id);
    }
}