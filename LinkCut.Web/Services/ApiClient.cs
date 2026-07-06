using System.Net.Http.Json;
using LinkCut.Web.Models;

namespace LinkCut.Web.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<ShortenedUrlResponse>> GetAllAsync()
    {
        return await _http.GetFromJsonAsync<List<ShortenedUrlResponse>>("api/urls")
               ?? new List<ShortenedUrlResponse>();
    }

    public async Task<ShortenedUrlResponse?> CreateAsync(string originalUrl)
    {
        var response = await _http.PostAsJsonAsync("api/urls", new { originalUrl });
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<ShortenedUrlResponse>();
    }
}
