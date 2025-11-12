using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class JwtBlacklistConfiguration : IEntityTypeConfiguration<JwtBlacklist>
{
    public void Configure(EntityTypeBuilder<JwtBlacklist> builder)
    {
        builder.ToTable("jwt_blacklist");
        
        builder.HasKey(e => e.JwtId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.JwtId)
            .HasColumnName("jwt_id");
        
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
        
        builder.Property(e => e.Token)
            .HasMaxLength(500)
            .HasColumnName("token");
        
        builder.Property(e => e.ExpiresAt)
            .HasColumnType("timestamp")
            .HasColumnName("expires_at");
        
        builder.HasOne(d => d.User)
            .WithMany(p => p.JwtBlacklists)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("jwt_blacklist_ibfk_1");
    }
}
