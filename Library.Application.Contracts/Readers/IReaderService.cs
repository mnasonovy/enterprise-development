namespace Library.Application.Contracts.Readers;

/// <summary>
/// Application service contract for working with readers.
/// </summary>
public interface IReaderService : IApplicationService
{
    public Task<ReaderDto?> GetAsync(int id);

    public Task<IReadOnlyList<ReaderDto>> GetListAsync();

    public Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input);

    public Task<ReaderDto> UpdateAsync(int id, ReaderCreateUpdateDto input);

    public Task DeleteAsync(int id);
}
