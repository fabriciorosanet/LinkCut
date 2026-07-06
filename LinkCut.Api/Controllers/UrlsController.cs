using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkCut.Api.Data;
using LinkCut.Api.Models;
using LinkCut.Api.Services;

namespace LinkCut.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlsController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly UrlShorteningService _shortener;

    public UrlsController(AppDbContext db, UrlShorteningService shortener)
    {
        _db = db;
        _shortener = shortener;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUrlRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            return BadRequest(new { error = "URL is required" });

        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != "http" && uri.Scheme != "https"))
            return BadRequest(new { error = "Invalid URL" });

        string shortCode;
        do
        {
            shortCode = _shortener.GenerateShortCode();
        } while (await _db.ShortenedUrls.AnyAsync(u => u.ShortCode == shortCode));

        var url = new ShortenedUrl
        {
            OriginalUrl = request.OriginalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow
        };

        _db.ShortenedUrls.Add(url);
        await _db.SaveChangesAsync();

        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        return Ok(new
        {
            url.Id,
            url.OriginalUrl,
            url.ShortCode,
            ShortUrl = $"{baseUrl}/{shortCode}",
            url.CreatedAt,
            url.ClickCount
        });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var urls = await _db.ShortenedUrls
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync();

        return Ok(urls);
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> GetByCode(string shortCode)
    {
        var url = await _db.ShortenedUrls
            .AsTracking()
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

        if (url is null)
            return NotFound(new { error = "Short URL not found" });

        url.ClickCount++;
        await _db.SaveChangesAsync();

        return Ok(url);
    }
}

public record CreateUrlRequest(string OriginalUrl);
