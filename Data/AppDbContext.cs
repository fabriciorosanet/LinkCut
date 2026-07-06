using Microsoft.EntityFrameworkCore;
using LinkCut.Api.Models;

namespace LinkCut.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<ShortenedUrl> ShortenedUrls => Set<ShortenedUrl>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(entity =>
        {
            entity.HasIndex(e => e.ShortCode).IsUnique();
        });
    }
}
