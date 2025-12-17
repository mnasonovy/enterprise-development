using AutoMapper;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над читателями.
/// Реализует интерфейс IReaderService и использует AutoMapper для преобразований DTO.
/// Делегирует работу с базой данных репозиторию ReaderRepository.
/// </summary>
public class ReaderService : IReaderService
{
    private readonly ReaderRepository _readerRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Инициализирует сервис с репозиторием и маппером.
    /// </summary>
    /// <param name="readerRepository">Репозиторий для работы с читателями в БД</param>
    /// <param name="mapper">AutoMapper для преобразования сущностей в DTO и обратно</param>
    public ReaderService(ReaderRepository readerRepository, IMapper mapper)
    {
        _readerRepository = readerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получить читателя по идентификатору.
    /// Загружает читателя с репозитория и преобразует в DTO через AutoMapper.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя</param>
    /// <returns>ReaderDto или null если читатель не найден</returns>
    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await _readerRepository.ReadAsync(id);
        return reader == null ? null : _mapper.Map<ReaderDto>(reader);
    }

    /// <summary>
    /// Получить список всех читателей.
    /// Загружает всех читателей с репозитория и преобразует в список DTO.
    /// </summary>
    /// <returns>Неизменяемый список ReaderDto всех читателей</returns>
    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await _readerRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<ReaderDto>>(readers);
    }

    /// <summary>
    /// Создать нового читателя в системе.
    /// Преобразует DTO в сущность, сохраняет в БД и возвращает созданный DTO.
    /// ID должен быть установлен вручную в DTO.
    /// </summary>
    /// <param name="input">DTO с данными нового читателя (Id, FullName, RegistrationDate обязательны)</param>
    /// <returns>ReaderDto созданного читателя с заполненным Id</returns>
    /// <exception cref="ArgumentException">Выбрасывается если ID не установлен или <= 0</exception>
    public async Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input)
    {
        if (input.Id <= 0)
            throw new ArgumentException("ID должен быть установлен вручную и быть больше 0", nameof(input.Id));

        var reader = _mapper.Map<Reader>(input);
        var created = await _readerRepository.CreateAsync(reader);
        return _mapper.Map<ReaderDto>(created);
    }

    /// <summary>
    /// Обновить существующего читателя.
    /// Загружает текущего читателя, применяет изменения, сохраняет в БД.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для обновления</param>
    /// <param name="input">DTO с новыми данными читателя</param>
    /// <returns>ReaderDto обновленного читателя или null если читатель не найден</returns>
    public async Task<ReaderDto?> UpdateAsync(int id, ReaderCreateUpdateDto input)
    {
        var existing = await _readerRepository.ReadAsync(id);
        if (existing == null)
            return null;

        _mapper.Map(input, existing);
        var updated = await _readerRepository.UpdateAsync(existing);
        return updated == null ? null : _mapper.Map<ReaderDto>(updated);
    }

    /// <summary>
    /// Удалить читателя по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор читателя для удаления</param>
    public async Task DeleteAsync(int id)
    {
        await _readerRepository.DeleteAsync(id);
    }
}
