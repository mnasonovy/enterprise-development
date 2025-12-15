namespace Library.Application.Contracts.Publishers;

/// <summary>
/// Application service contract for working with publishers.
/// </summary>
public interface IPublisherService : IApplicationService
{
    public Task<PublisherDto?> GetAsync(int id);

    public Task<IReadOnlyList<PublisherDto>> GetListAsync();

    public Task<PublisherDto> CreateAsync(PublisherCreateUpdateDto input);

    public Task<PublisherDto> UpdateAsync(int id, PublisherCreateUpdateDto input);

    public Task DeleteAsync(int id);
}
