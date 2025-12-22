using AutoMapper;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над читателями.
/// Реализует интерфейс IReaderService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторию через интерфейс.
/// </summary>
public class ReaderService(IReaderRepository readerRepository, IMapper mapper) : IReaderService
{
    /// <summary>
    /// Получить читателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>ReaderDto или null если читатель не найден</returns>
    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await readerRepository.ReadAsync(id);
        return reader == null ? null : mapper.Map<ReaderDto>(reader);
    }

    /// <summary>
    /// Получить список всех читателей.
    /// </summary>
    /// <returns>Неизменяемый список ReaderDto всех читателей</returns>
    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await readerRepository.ReadAllAsync();
        return mapper.Map<IReadOnlyList<ReaderDto>>(readers);
    }

    /// <summary>
    /// Создать нового читателя.
    /// </summary>
    /// <param name="input">DTO с данными нового читателя</param>
    /// <returns>ReaderDto созданного читателя с заполненным Id</returns>
    public async Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input)
    {
        var reader = mapper.Map<Reader>(input);
        var created = await readerRepository.CreateAsync(reader);
        return mapper.Map<ReaderDto>(created);
    }

    /// <summary>
    /// Обновить существующего читателя.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для обновления</param>
    /// <param name="input">DTO с новыми данными читателя</param>
    /// <returns>ReaderDto обновленного читателя или null если читатель не найден</returns>
    public async Task<ReaderDto?> UpdateAsync(int id, ReaderCreateUpdateDto input)
    {
        var existing = await readerRepository.ReadAsync(id);
        if (existing == null)
            return null;

        mapper.Map(input, existing);
        var updated = await readerRepository.UpdateAsync(existing);
        return updated == null ? null : mapper.Map<ReaderDto>(updated);
    }

    /// <summary>
    /// Удалить читателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await readerRepository.DeleteAsync(id);
    }
}