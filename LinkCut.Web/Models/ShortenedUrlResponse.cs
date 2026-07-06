using System.Text.Json.Serialization;

namespace LinkCut.Web.Models;

public class ShortenedUrlResponse
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("originalUrl")]
    public string OriginalUrl { get; set; } = string.Empty;

    [JsonPropertyName("shortCode")]
    public string ShortCode { get; set; } = string.Empty;

    [JsonPropertyName("shortUrl")]
    public string ShortUrl { get; set; } = string.Empty;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("clickCount")]
    public int ClickCount { get; set; }
}
