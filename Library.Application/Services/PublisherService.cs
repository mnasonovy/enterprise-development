using Library.Application.Contracts.Publishers;
using Library.Domain.Models;
using Library.Infrastructure.MongoDb.Repositories;

namespace Library.Application.Services;

public class PublisherService : IPublisherService
{
    private readonly PublisherMongoRepository _publisherRepository;

    public PublisherService(PublisherMongoRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<PublisherDto?> GetAsync(int id)
    {
        var publisher = await _publisherRepository.ReadAsync(id);
        return publisher is null
            ? null
            : MapToDto(publisher);
    }

    public async Task<IReadOnlyList<PublisherDto>> GetListAsync()
    {
        var publishers = await _publisherRepository.ReadAllAsync();
        return publishers.Select(MapToDto).ToArray();
    }

    public async Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input)
    {
        var publisher = new Publisher
        {
            Name = input.Name
        };

        var created = await _publisherRepository.CreateAsync(publisher);
        return MapToDto(created);
    }

    public async Task<PublisherDto> UpdateAsync(int id, PublisherCreateUpdateDto input)
    {
        var existing = await _publisherRepository.ReadAsync(id)
            ?? throw new InvalidOperationException($"Publisher with id {id} was not found.");

        existing.Name = input.Name;

        var updated = await _publisherRepository.UpdateAsync(existing)
            ?? throw new InvalidOperationException($"Publisher with id {id} was not updated.");

        return MapToDto(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var deleted = await _publisherRepository.DeleteAsync(id);
        if (!deleted)
        {
            throw new InvalidOperationException($"Publisher with id {id} was not deleted.");
        }
    }

    private static PublisherDto MapToDto(Publisher publisher) => new()
    {
        Id = publisher.Id,
        Name = publisher.Name
    };
}