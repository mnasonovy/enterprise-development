using AutoMapper;
using Library.Application.Contracts.Publishers;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над издателями.
/// 🔧 ИСПРАВЛЕНО: Используется AutoMapper!
/// </summary>
public class PublisherService : IPublisherService
{
    private readonly PublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    public PublisherService(PublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить издателя по идентификатору.
    /// 🔧 Include загружает Books!
    /// </summary>
    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await _publisherRepository.ReadAsync(id);
        return publisher == null ? null : _mapper.Map<PublisherDto>(publisher);
    }

    /// <summary>
    /// Получить список всех издателей.
    /// 🔧 Include загружает Books!
    /// </summary>
    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await _publisherRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<PublisherDto>>(publishers);
    }

    /// <summary>
    /// Создать нового издателя.
    /// </summary>
    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        var publisher = _mapper.Map<Publisher>(input);
        var created = await _publisherRepository.CreateAsync(publisher);
        return _mapper.Map<PublisherDto>(created);
    }

    /// <summary>
    /// Обновить существующего издателя.
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
    /// Удалить издателя по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _publisherRepository.DeleteAsync(id);
    }
}
