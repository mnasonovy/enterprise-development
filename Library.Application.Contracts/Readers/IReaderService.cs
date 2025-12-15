namespace Library.Application.Contracts.Readers;

/// <summary>
/// Application service contract for working with readers.
/// </summary>
public interface IReaderService : IApplicationService
{
    Task<ReaderDto?> GetAsync(int id);

    Task<IReadOnlyList<ReaderDto>> GetListAsync();

    Task<ReaderDto> CreateAsync(ReaderCreateUpdateDto input);

    Task<ReaderDto> UpdateAsync(int id, ReaderCreateUpdateDto input);

    Task DeleteAsync(int id);
}
