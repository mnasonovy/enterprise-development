using Library.Application.Contracts.Readers;
using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Repositories;

namespace Library.Application.Services;

public class ReaderService : IReaderService
{
    private readonly ReaderMongoRepository _readerRepository;

    public ReaderService(ReaderMongoRepository readerRepository)
    {
        _readerRepository = readerRepository;
    }

    public async Task<ReaderDto?> GetAsync(int id)
    {
        var reader = await _readerRepository.ReadAsync(id);
        return reader is null
            ? null
            : MapToDto(reader);
    }

    public async Task<IReadOnlyList<ReaderDto>> GetListAsync()
    {
        var readers = await _readerRepository.ReadAllAsync();
        return readers.Select(MapToDto).ToArray();
    }

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

    public async Task DeleteAsync(int id)
    {
        var deleted = await _readerRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Reader with id {id} was not deleted.");
        }
    }

    private static ReaderDto MapToDto(Reader reader) => new()
    {
        Id = reader.Id,
        FullName = reader.FullName,
        Address = reader.Address,
        Phone = reader.Phone,
        RegistrationDate = reader.RegistrationDate
    };
}
