using AutoMapper;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;
using Library.Infrastructure.MongoEf.Repositories;

namespace Library.Application.Services;

/// <summary>
/// Сервис для CRUD-операций над читателями.
/// 🔧 ИСПРАВЛЕНО: Используется AutoMapper!
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
    /// Получить читателя по идентификатору.
    /// 🔧 Include загружает Issues!
    /// </summary>
    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await _readerRepository.ReadAsync(id);
        return reader == null ? null : _mapper.Map<ReaderDto>(reader);
    }

    /// <summary>
    /// Получить список всех читателей.
    /// 🔧 Include загружает Issues!
    /// </summary>
    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await _readerRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<ReaderDto>>(readers);
    }

    /// <summary>
    /// Создать нового читателя.
    /// </summary>
    public async Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input)
    {
        var reader = _mapper.Map<Reader>(input);
        var created = await _readerRepository.CreateAsync(reader);
        return _mapper.Map<ReaderDto>(created);
    }

    /// <summary>
    /// Обновить существующего читателя.
    /// </summary>
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
    public async Task DeleteAsync(int id)
    {
        await _readerRepository.DeleteAsync(id);
    }
}
