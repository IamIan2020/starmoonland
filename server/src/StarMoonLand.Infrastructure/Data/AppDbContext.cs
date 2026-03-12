using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StarMoonLand.Core.Entities;

namespace StarMoonLand.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<PageCategory> PageCategories => Set<PageCategory>();
    public DbSet<Page> Pages => Set<Page>();
    public DbSet<PageSlide> PageSlides => Set<PageSlide>();
    public DbSet<PageTab> PageTabs => Set<PageTab>();
    public DbSet<NewsCategory> NewsCategories => Set<NewsCategory>();
    public DbSet<News> News => Set<News>();
    public DbSet<AlbumCategory> AlbumCategories => Set<AlbumCategory>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<AlbumPhoto> AlbumPhotos => Set<AlbumPhoto>();
    public DbSet<ContactSubmission> ContactSubmissions => Set<ContactSubmission>();
    public DbSet<TrafficInfo> TrafficInfos => Set<TrafficInfo>();
    public DbSet<HomepageSlide> HomepageSlides => Set<HomepageSlide>();
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // ApplicationUser
        builder.Entity<ApplicationUser>(e =>
        {
            e.Property(u => u.DisplayName).HasMaxLength(100).IsRequired();
        });

        // PageCategory
        builder.Entity<PageCategory>(e =>
        {
            e.HasIndex(c => c.Slug).IsUnique();
            e.Property(c => c.Slug).HasMaxLength(50).IsRequired();
            e.Property(c => c.TitleZh).HasMaxLength(100).IsRequired();
            e.Property(c => c.TitleEn).HasMaxLength(100).IsRequired();
            e.Property(c => c.BannerImage).HasMaxLength(500);
        });

        // Page
        builder.Entity<Page>(e =>
        {
            e.HasIndex(p => new { p.CategoryId, p.Slug }).IsUnique();
            e.Property(p => p.Slug).HasMaxLength(50).IsRequired();
            e.Property(p => p.Title).HasMaxLength(200).IsRequired();
            e.Property(p => p.Subtitle).HasMaxLength(500);
            e.Property(p => p.MetaTitle).HasMaxLength(200);
            e.Property(p => p.MetaDescription).HasMaxLength(500);
            e.HasOne(p => p.Category).WithMany(c => c.Pages).HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Cascade);
        });

        // PageSlide
        builder.Entity<PageSlide>(e =>
        {
            e.Property(s => s.ImageUrl).HasMaxLength(500).IsRequired();
            e.Property(s => s.Title).HasMaxLength(200);
            e.Property(s => s.Description).HasMaxLength(1000);
            e.HasOne(s => s.Page).WithMany(p => p.Slides).HasForeignKey(s => s.PageId).OnDelete(DeleteBehavior.Cascade);
        });

        // PageTab
        builder.Entity<PageTab>(e =>
        {
            e.Property(t => t.Title).HasMaxLength(200).IsRequired();
            e.HasOne(t => t.Page).WithMany(p => p.Tabs).HasForeignKey(t => t.PageId).OnDelete(DeleteBehavior.Cascade);
        });

        // NewsCategory
        builder.Entity<NewsCategory>(e =>
        {
            e.HasIndex(c => c.Slug).IsUnique();
            e.Property(c => c.Name).HasMaxLength(100).IsRequired();
            e.Property(c => c.Slug).HasMaxLength(50).IsRequired();
        });

        // News
        builder.Entity<News>(e =>
        {
            e.Property(n => n.Title).HasMaxLength(300).IsRequired();
            e.Property(n => n.Summary).HasMaxLength(1000);
            e.Property(n => n.CoverImage).HasMaxLength(500);
            e.HasOne(n => n.Category).WithMany(c => c.NewsItems).HasForeignKey(n => n.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        // AlbumCategory
        builder.Entity<AlbumCategory>(e =>
        {
            e.HasIndex(c => c.Slug).IsUnique();
            e.Property(c => c.Name).HasMaxLength(100).IsRequired();
            e.Property(c => c.Slug).HasMaxLength(50).IsRequired();
        });

        // Album
        builder.Entity<Album>(e =>
        {
            e.Property(a => a.Title).HasMaxLength(200).IsRequired();
            e.Property(a => a.Description).HasMaxLength(1000);
            e.Property(a => a.CoverImage).HasMaxLength(500);
            e.HasOne(a => a.Category).WithMany(c => c.Albums).HasForeignKey(a => a.CategoryId).OnDelete(DeleteBehavior.Restrict);
        });

        // AlbumPhoto
        builder.Entity<AlbumPhoto>(e =>
        {
            e.Property(p => p.ImageUrl).HasMaxLength(500).IsRequired();
            e.Property(p => p.ThumbnailUrl).HasMaxLength(500);
            e.Property(p => p.Caption).HasMaxLength(500);
            e.HasOne(p => p.Album).WithMany(a => a.Photos).HasForeignKey(p => p.AlbumId).OnDelete(DeleteBehavior.Cascade);
        });

        // ContactSubmission
        builder.Entity<ContactSubmission>(e =>
        {
            e.Property(c => c.Name).HasMaxLength(100).IsRequired();
            e.Property(c => c.Phone).HasMaxLength(50).IsRequired();
            e.Property(c => c.Email).HasMaxLength(200).IsRequired();
            e.Property(c => c.Message).HasMaxLength(5000).IsRequired();
            e.Property(c => c.ReplyNote).HasMaxLength(1000);
            e.Property(c => c.ReplyContent).HasMaxLength(5000);
        });

        // TrafficInfo
        builder.Entity<TrafficInfo>(e =>
        {
            e.Property(t => t.TabName).HasMaxLength(100).IsRequired();
        });

        // HomepageSlide
        builder.Entity<HomepageSlide>(e =>
        {
            e.Property(s => s.ImageUrl).HasMaxLength(500).IsRequired();
            e.Property(s => s.AltText).HasMaxLength(200);
            e.Property(s => s.LinkUrl).HasMaxLength(500);
        });

        // SiteSetting
        builder.Entity<SiteSetting>(e =>
        {
            e.HasIndex(s => s.Key).IsUnique();
            e.Property(s => s.Key).HasMaxLength(100).IsRequired();
            e.Property(s => s.Value).HasMaxLength(2000);
            e.Property(s => s.Description).HasMaxLength(500);
        });

        // MediaFile
        builder.Entity<MediaFile>(e =>
        {
            e.Property(f => f.Filename).HasMaxLength(300).IsRequired();
            e.Property(f => f.OriginalName).HasMaxLength(300).IsRequired();
            e.Property(f => f.ContentType).HasMaxLength(100).IsRequired();
            e.Property(f => f.FilePath).HasMaxLength(500).IsRequired();
            e.Property(f => f.ThumbnailPath).HasMaxLength(500);
        });
    }
}
