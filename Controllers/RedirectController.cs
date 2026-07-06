using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LinkCut.Api.Data;

namespace LinkCut.Api.Controllers;

[ApiController]
public class RedirectController : ControllerBase
{
    private readonly AppDbContext _db;

    public RedirectController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var url = await _db.ShortenedUrls
            .AsTracking()
            .FirstOrDefaultAsync(u => u.ShortCode == shortCode);

        if (url is null)
            return NotFound("Short URL not found");

        url.ClickCount++;
        await _db.SaveChangesAsync();

        return RedirectPermanent(url.OriginalUrl);
    }
}
