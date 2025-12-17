using AutoMapper;
using Library.Application.Contracts.Publishers;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления издателями.
/// Реализует интерфейс IPublisherService, обеспечивая выполнение CRUD операций над издателями.
/// Использует AutoMapper для преобразования между Domain моделями и DTO.
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
    /// Получает издателя по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор издателя для поиска.</param>
    /// <returns>DTO издателя, если найден; null если издатель не существует.</returns>
    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await _publisherRepository.ReadAsync(id);
        return publisher == null ? null : _mapper.Map<PublisherDto>(publisher);
    }

    /// <summary>
    /// Получает список всех издателей.
    /// </summary>
    /// <returns>Коллекция DTO всех издателей. Если издателей нет, возвращает пустой список.</returns>
    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await _publisherRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<PublisherDto>>(publishers);
    }

    /// <summary>
    /// Создаёт нового издателя в базе данных.
    /// </summary>
    /// <param name="input">DTO с данными нового издателя (Name обязателен).</param>
    /// <returns>DTO созданного издателя с назначенным идентификатором.</returns>
    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        var publisher = _mapper.Map<Publisher>(input);
        var created = await _publisherRepository.CreateAsync(publisher);
        return _mapper.Map<PublisherDto>(created);
    }

    /// <summary>
    /// Обновляет информацию об существующем издателе.
    /// </summary>
    /// <param name="id">Идентификатор издателя для обновления.</param>
    /// <param name="input">DTO с новыми данными издателя.</param>
    /// <returns>Обновленный DTO издателя, если успешно; null если издатель не найден.</returns>
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
    /// Удаляет издателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор издателя для удаления.</param>
    public async Task DeleteAsync(int id)
    {
        await _publisherRepository.DeleteAsync(id);
    }
}
