namespace Library.Application.Contracts.Publishers;

/// <summary>
/// Application service contract for working with publishers.
/// </summary>
public interface IPublisherService : IApplicationService
{
    Task<PublisherDto?> GetAsync(int id);

    Task<IReadOnlyList<PublisherDto>> GetListAsync();

    Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input);

    Task<PublisherDto> UpdateAsync(int id, PublisherCreateUpdateDto input);

    Task DeleteAsync(int id);
}
