using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using tkNews.Domain.Entities.Identity;

namespace tkNews.Infrastructure.Data.Identity;

public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(500);
        });
        
        builder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);
        });
        
        builder.Entity<RefreshToken>(entity =>
        {
            entity.HasOne(rt => rt.User)
                .WithMany()
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
} 