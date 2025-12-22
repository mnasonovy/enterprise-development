using AutoMapper;
using Library.Application.Contracts.Publishers;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над издателями.
/// Реализует интерфейс IPublisherService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторию через интерфейс.
/// </summary>
public class PublisherService(IPublisherRepository publisherRepository, IMapper mapper) : IPublisherService
{
    /// <summary>
    /// Получить издателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>PublisherDto или null если издатель не найден</returns>
    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await publisherRepository.ReadAsync(id);
        return publisher == null ? null : mapper.Map<PublisherDto>(publisher);
    }

    /// <summary>
    /// Получить список всех издателей.
    /// </summary>
    /// <returns>Неизменяемый список PublisherDto всех издателей</returns>
    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await publisherRepository.ReadAllAsync();
        return mapper.Map<IReadOnlyList<PublisherDto>>(publishers);
    }

    /// <summary>
    /// Создать нового издателя.
    /// </summary>
    /// <param name="input">DTO с данными нового издателя</param>
    /// <returns>PublisherDto созданного издателя с заполненным Id</returns>
    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        var publisher = mapper.Map<Publisher>(input);
        var created = await publisherRepository.CreateAsync(publisher);
        return mapper.Map<PublisherDto>(created);
    }

    /// <summary>
    /// Обновить существующего издателя.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для обновления</param>
    /// <param name="input">DTO с новыми данными издателя</param>
    /// <returns>PublisherDto обновленного издателя или null если издатель не найден</returns>
    public async Task<PublisherDto?> UpdateAsync(int id, PublisherCreateUpdateDto input)
    {
        var existing = await publisherRepository.ReadAsync(id);
        if (existing == null)
            return null;

        mapper.Map(input, existing);
        var updated = await publisherRepository.UpdateAsync(existing);
        return updated == null ? null : mapper.Map<PublisherDto>(updated);
    }

    /// <summary>
    /// Удалить издателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await publisherRepository.DeleteAsync(id);
    }
}