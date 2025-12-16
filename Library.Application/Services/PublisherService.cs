namespace Library.Application.Services;

using Library.Application.Contracts.Publishers;

using Library.Domain.Models;

using Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Сервис для CRUD-операций над издателями
/// </summary>
public class PublisherService : IPublisherService
{
    private readonly PublisherRepository _publisherRepository;

    public PublisherService(PublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    /// <summary>
    /// Получить издателя по идентификатору
    /// </summary>
    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await _publisherRepository.ReadAsync(id);
        return publisher is null
            ? null
            : MapToDto(publisher);
    }

    /// <summary>
    /// Получить список всех издателей
    /// </summary>
    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await _publisherRepository.ReadAllAsync();
        return publishers.Select(MapToDto).ToList().AsReadOnly();
    }

    /// <summary>
    /// Создать нового издателя
    /// </summary>
    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        var publisher = new Publisher
        {
            Name = input.Name
        };

        var created = await _publisherRepository.CreateAsync(publisher);
        return MapToDto(created);
    }

    /// <summary>
    /// Обновить существующего издателя
    /// </summary>
    public async Task<PublisherDto> UpdateAsync(int id, PublisherCreateUpdateDto input)
    {
        var existing = await _publisherRepository.ReadAsync(id)
            ?? throw new InvalidOperationException($"Publisher with id {id} was not found.");

        existing.Name = input.Name;

        var updated = await _publisherRepository.UpdateAsync(existing)
            ?? throw new InvalidOperationException($"Publisher with id {id} was not updated.");

        return MapToDto(updated);
    }

    /// <summary>
    /// Удалить издателя по идентификатору
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var deleted = await _publisherRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Publisher with id {id} was not deleted.");
        }
    }

    /// <summary>
    /// Преобразовать Domain модель издателя в DTO
    /// </summary>
    private static PublisherDto MapToDto(Publisher publisher) => new()
    {
        Id = publisher.Id,
        Name = publisher.Name
    };
}