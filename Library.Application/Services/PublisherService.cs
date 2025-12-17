using AutoMapper;
using Library.Application.Contracts.Publishers;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над издателями.
/// Реализует интерфейс IPublisherService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторию PublisherRepository.
/// </summary>
public class PublisherService : IPublisherService
{
    private readonly PublisherRepository _publisherRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует сервис с репозиторием и маппером.
    /// </summary>
    /// <param name="publisherRepository">Репозиторий для работы с издателями в БД</param>
    /// <param name="mapper">AutoMapper для преобразования сущностей в DTO и обратно</param>
    public PublisherService(PublisherRepository publisherRepository, IMapper mapper)
    {
        _publisherRepository = publisherRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить издателя по идентификатору.
    /// Загружает издателя с репозитория и преобразует в DTO через AutoMapper.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя</param>
    /// <returns>PublisherDto или null если издатель не найден</returns>
    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await _publisherRepository.ReadAsync(id);
        return publisher == null ? null : _mapper.Map<PublisherDto>(publisher);
    }

    /// <summary>
    /// Получить список всех издателей.
    /// Загружает все издателей с репозитория и преобразует в список DTO.
    /// </summary>
    /// <returns>Неизменяемый список PublisherDto всех издателей</returns>
    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await _publisherRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<PublisherDto>>(publishers);
    }

    /// <summary>
    /// Создать нового издателя в системе.
    /// Преобразует DTO в сущность, сохраняет в БД и возвращает созданный DTO.
    /// ID должен быть установлен вручную в DTO.
    /// </summary>
    /// <param name="input">DTO с данными нового издателя (Name и Id обязательны)</param>
    /// <returns>PublisherDto созданного издателя с заполненным Id</returns>
    /// <exception cref="ArgumentException">Выбрасывается если ID не установлен или <= 0</exception>
    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        if (input.Id <= 0)
            throw new ArgumentException("ID должен быть установлен вручную и быть больше 0", nameof(input.Id));

        var publisher = _mapper.Map<Publisher>(input);
        var created = await _publisherRepository.CreateAsync(publisher);
        return _mapper.Map<PublisherDto>(created);
    }

    /// <summary>
    /// Обновить существующего издателя.
    /// Загружает текущего издателя, применяет изменения, сохраняет в БД.
    /// </summary>
    /// <param name="id">Уникальный идентификатор издателя для обновления</param>
    /// <param name="input">DTO с новыми данными издателя</param>
    /// <returns>PublisherDto обновленного издателя или null если издатель не найден</returns>
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
    /// <param name="id">Уникальный идентификатор издателя для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await _publisherRepository.DeleteAsync(id);
    }
}
