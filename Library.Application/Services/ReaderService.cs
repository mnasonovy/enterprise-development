namespace Library.Application.Services;

using Library.Application.Contracts.Readers;

using Library.Domain.Models;

using Library.Infrastructure.MongoEf.Repositories;

/// <summary>
/// Сервис для CRUD-операций над читателями
/// </summary>
public class ReaderService : IReaderService
{
    private readonly ReaderRepository _readerRepository;

    public ReaderService(ReaderRepository readerRepository)
    {
        _readerRepository = readerRepository;
    }

    /// <summary>
    /// Получить читателя по идентификатору
    /// </summary>
    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await _readerRepository.ReadAsync(id);
        return reader is null
            ? null
            : MapToDto(reader);
    }

    /// <summary>
    /// Получить список всех читателей
    /// </summary>
    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await _readerRepository.ReadAllAsync();
        return readers.Select(MapToDto).ToList().AsReadOnly();
    }

    /// <summary>
    /// Создать нового читателя
    /// </summary>
    public async Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input)
    {
        var reader = new Reader
        {
            FullName = input.FullName,
            Address = input.Address,
            Phone = input.Phone,
            RegistrationDate = input.RegistrationDate
        };

        var created = await _readerRepository.CreateAsync(reader);
        return MapToDto(created);
    }

    /// <summary>
    /// Обновить существующего читателя
    /// </summary>
    public async Task<ReaderDto> UpdateAsync(int id, ReaderCreateUpdateDto input)
    {
        var existing = await _readerRepository.ReadAsync(id)
            ?? throw new InvalidOperationException($"Reader with id {id} was not found.");

        existing.FullName = input.FullName;
        existing.Address = input.Address;
        existing.Phone = input.Phone;
        existing.RegistrationDate = input.RegistrationDate;

        var updated = await _readerRepository.UpdateAsync(existing)
            ?? throw new InvalidOperationException($"Reader with id {id} was not updated.");

        return MapToDto(updated);
    }

    /// <summary>
    /// Удалить читателя по идентификатору
    /// </summary>
    public async Task DeleteAsync(int id)
    {
        var deleted = await _readerRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Reader with id {id} was not deleted.");
        }
    }

    /// <summary>
    /// Преобразовать Domain модель читателя в DTO
    /// </summary>
    private static ReaderDto MapToDto(Reader reader) => new()
    {
        Id = reader.Id,
        FullName = reader.FullName,
        Address = reader.Address,
        Phone = reader.Phone,
        RegistrationDate = reader.RegistrationDate
    };
}