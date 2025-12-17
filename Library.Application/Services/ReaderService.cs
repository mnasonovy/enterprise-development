using AutoMapper;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления читателями.
/// Реализует интерфейс IReaderService, обеспечивая выполнение CRUD операций над читателями.
/// Использует AutoMapper для преобразования между Domain моделями и DTO.
/// </summary>
public class ReaderService : IReaderService
{
    private readonly ReaderRepository _readerRepository;
    private readonly IMapper _mapper;

    public ReaderService(ReaderRepository readerRepository, IMapper mapper)
    {
        _readerRepository = readerRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получает читателя по уникальному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор читателя для поиска.</param>
    /// <returns>DTO читателя, если найден; null если читатель не существует.</returns>
    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await _readerRepository.ReadAsync(id);
        return reader == null ? null : _mapper.Map<ReaderDto>(reader);
    }

    /// <summary>
    /// Получает список всех читателей.
    /// </summary>
    /// <returns>Коллекция DTO всех читателей. Если читателей нет, возвращает пустой список.</returns>
    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await _readerRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<ReaderDto>>(readers);
    }

    /// <summary>
    /// Создаёт нового читателя в базе данных.
    /// </summary>
    /// <param name="input">DTO с данными нового читателя (FullName, Address, Phone, RegistrationDate обязательны).</param>
    /// <returns>DTO созданного читателя с назначенным идентификатором.</returns>
    public async Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input)
    {
        var reader = _mapper.Map<Reader>(input);
        var created = await _readerRepository.CreateAsync(reader);
        return _mapper.Map<ReaderDto>(created);
    }

    /// <summary>
    /// Обновляет информацию об существующем читателе.
    /// </summary>
    /// <param name="id">Идентификатор читателя для обновления.</param>
    /// <param name="input">DTO с новыми данными читателя.</param>
    /// <returns>Обновленный DTO читателя, если успешно; null если читатель не найден.</returns>
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
    /// Удаляет читателя из базы данных по идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор читателя для удаления.</param>
    public async Task DeleteAsync(int id)
    {
        await _readerRepository.DeleteAsync(id);
    }
}
