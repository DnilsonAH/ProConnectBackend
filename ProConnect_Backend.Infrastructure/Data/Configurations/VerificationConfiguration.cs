using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class VerificationConfiguration : IEntityTypeConfiguration<Verification>
{
    public void Configure(EntityTypeBuilder<Verification> builder)
    {
        builder.ToTable("verifications");
        
        builder.HasKey(e => e.VerificationId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.VerificationId)
            .HasColumnName("verification_id");
        
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
        
        builder.Property(e => e.Status)
            .HasMaxLength(50)
            .HasColumnName("status");
        
        builder.Property(e => e.VerificationDate)
            .HasColumnType("timestamp")
            .HasColumnName("verification_date");
        
        builder.Property(e => e.Notes)
            .HasColumnType("text")
            .HasColumnName("notes");
        
        builder.Property(e => e.CreatedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("created_at");
        
        builder.HasOne(d => d.User)
            .WithMany(p => p.Verifications)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("verifications_ibfk_1");
    }
}
