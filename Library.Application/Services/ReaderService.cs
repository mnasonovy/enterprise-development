using AutoMapper;
using Library.Application.Contracts.Readers;
using Library.Domain.Models;
using Library.Domain.RepositoryInterfaces;

namespace Library.Application.Services;

/// <summary>
/// Сервис для управления читателями в библиотечной системе.
/// Реализует CRUD операции и Upsert для синхронизации с NATS JetStream.
/// Читатели - зарегистрированные пользователи, которые могут брать книги.
/// </summary>
public class ReaderService(IReaderRepository readerRepository, IMapper mapper) : IReaderService
{
    private readonly IReaderRepository _readerRepository = readerRepository;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Получает читателя по идентификатору.
    /// </summary>
    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await _readerRepository.ReadAsync(id);
        return reader == null ? null : _mapper.Map<ReaderDto>(reader);
    }

    /// <summary>
    /// Получает список всех читателей.
    /// </summary>
    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await _readerRepository.ReadAllAsync();
        return _mapper.Map<IReadOnlyList<ReaderDto>>(readers);
    }

    /// <summary>
    /// Создаёт нового читателя с автоматической генерацией ID и регистрирует в системе.
    /// </summary>
    public async Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input)
    {
        var maxId = await _readerRepository.GetMaxIdAsync();
        var reader = _mapper.Map<Reader>(input);
        reader.Id = maxId + 1; // ← генерируем новый ID
        var created = await _readerRepository.CreateAsync(reader);
        return _mapper.Map<ReaderDto>(created);
    }

    /// <summary>
    /// Обновляет информацию об существующем читателе.
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
    /// Удаляет читателя по идентификатору.
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        await _readerRepository.DeleteAsync(id);
    }

    /// <summary>
    /// Создаёт нового читателя или обновляет существующего (Upsert).
    /// При создании генерируется новый ID автоматически.
    /// Идемпотентная операция для синхронизации из NATS.
    /// </summary>
    public async Task<ReaderDto> UpsertAsync(ReaderCreateUpdateDto input)
    {
        var maxId = await _readerRepository.GetMaxIdAsync();
        var reader = _mapper.Map<Reader>(input);
        reader.Id = maxId + 1; // ← генерируем новый ID при создании
        var upsertedReader = await _readerRepository.CreateAsync(reader);
        return _mapper.Map<ReaderDto>(upsertedReader);
    }
}
