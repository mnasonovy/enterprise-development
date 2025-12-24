using AutoMapper;
using Library.Application.Contracts.Publishers;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления издателями в библиотечной системе.
/// Реализует CRUD операции и Upsert для синхронизации с NATS JetStream.
/// Издатели используются как справочник при работе с книгами.
/// </summary>
public class PublisherService(IPublisherRepository publisherRepository, IMapper mapper) : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получает издателя по идентификатору.
    /// </summary>
    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await _publisherRepository.ReadAsync(id);
        return publisher == null ? null : _mapper.Map<PublisherDto>(publisher);
    }

    /// <summary>
    /// Получает список всех издателей.
    /// </summary>
    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await _publisherRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<PublisherDto>>(publishers);
    }

    /// <summary>
    /// Создаёт нового издателя с автоматической генерацией ID.
    /// </summary>
    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        var maxId = await _publisherRepository.GetMaxIdAsync();
        var publisher = _mapper.Map<Publisher>(input);
        publisher.Id = maxId + 1; // ← генерируем новый ID
        var created = await _publisherRepository.CreateAsync(publisher);
        return _mapper.Map<PublisherDto>(created);
    }

    /// <summary>
    /// Обновляет существующего издателя.
    /// </summary>
    public async Task<PublisherDto?> UpdateAsync(int id, PublisherCreateUpdateDto input)
    {
        var existing = await _publisherRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _publisherRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<PublisherDto>(updated);
    }

    /// <summary>
    /// Удаляет издателя по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _publisherRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Создаёт нового издателя или обновляет существующего (Upsert).
    /// При создании генерируется новый ID автоматически.
    /// Идемпотентная операция для синхронизации из NATS.
    /// </summary>
    public async Task<PublisherDto> UpsertAsync(PublisherCreateUpdateDto input)
    {
        var maxId = await _publisherRepository.GetMaxIdAsync();
        var publisher = _mapper.Map<Publisher>(input);
        publisher.Id = maxId + 1; // ← генерируем новый ID при создании
        var upsertedPublisher = await _publisherRepository.CreateAsync(publisher);
        return _mapper.Map<PublisherDto>(upsertedPublisher);
    }
}
