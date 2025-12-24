using Library.Application.Contracts.Issues;
using LibraryClient.Models;
using System.Net.Http.Json;

namespace LibraryClient.OpenApis;

public class LibraryApiWrapper
{
    private readonly HttpClient _http;
    private const int MaxRetries = 5;
    private const int DelayMs = 1000;

    public LibraryApiWrapper(HttpClient httpClient)
    {
        httpClient.BaseAddress ??= new Uri("https://localhost:7000");
        _http = httpClient;
    }

    private static async Task<T?> FetchWithRetryAsync<T>(Func<Task<T?>> fetchFunc, CancellationToken ct = default)
    {
        for (var i = 0; i < MaxRetries; i++)
        {
            try
            {
                return await fetchFunc();
            }
            catch (HttpRequestException) when (i < MaxRetries - 1)
            {
                await Task.Delay(DelayMs, ct);
            }
        }
        // Последняя попытка без обработки
        return await fetchFunc();
    }

    // --- Books ---
    public async Task<List<BookClientDto>> GetBooksAsync(CancellationToken ct = default)
    {
        var result = await FetchWithRetryAsync(
            () => _http.GetFromJsonAsync<List<BookClientDto>>("/api/Books", ct),
            ct);
        return result ?? [];
    }

    public async Task<BookClientDto?> CreateBookAsync(BookClientDto book, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("/api/Books", book, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookClientDto>(ct);
    }

    public async Task<BookClientDto?> UpdateBookAsync(int id, BookClientDto book, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"/api/Books/{id}", book, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<BookClientDto>(ct);
    }

    public async Task DeleteBookAsync(int id, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"/api/Books/{id}", ct);
        response.EnsureSuccessStatusCode();
    }

    // --- Readers ---
    public async Task<List<ReaderClientDto>> GetReadersAsync(CancellationToken ct = default)
    {
        var result = await FetchWithRetryAsync(
            () => _http.GetFromJsonAsync<List<ReaderClientDto>>("/api/Readers", ct),
            ct);
        return result ?? [];
    }

    public async Task<ReaderClientDto?> CreateReaderAsync(ReaderClientDto reader, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("/api/Readers", reader, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReaderClientDto>(ct);
    }

    public async Task<ReaderClientDto?> UpdateReaderAsync(int id, ReaderClientDto reader, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"/api/Readers/{id}", reader, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ReaderClientDto>(ct);
    }

    public async Task DeleteReaderAsync(int id, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"/api/Readers/{id}", ct);
        response.EnsureSuccessStatusCode();
    }

    // --- Issues ---
    public async Task<List<IssueDto>> GetIssuesAsync(CancellationToken ct = default)
    {
        var result = await FetchWithRetryAsync(
            () => _http.GetFromJsonAsync<List<IssueDto>>("/api/Issues", ct),
            ct);
        return result ?? [];
    }

    public async Task<IssueDto?> CreateIssueAsync(IssueCreateUpdateDto dto, CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync("/api/Issues", dto, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IssueDto>(ct);
    }

    public async Task<IssueDto?> UpdateIssueAsync(int id, IssueCreateUpdateDto dto, CancellationToken ct = default)
    {
        var response = await _http.PutAsJsonAsync($"/api/Issues/{id}", dto, ct);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<IssueDto>(ct);
    }

    public async Task DeleteIssueAsync(int id, CancellationToken ct = default)
    {
        var response = await _http.DeleteAsync($"/api/Issues/{id}", ct);
        response.EnsureSuccessStatusCode();
    }
}
