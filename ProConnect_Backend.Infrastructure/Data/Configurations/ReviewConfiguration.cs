using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("reviews");
        
        builder.HasKey(e => e.ReviewId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.ReviewId)
            .HasColumnName("review_id");
        
        builder.Property(e => e.SessionId)
            .HasColumnName("session_id");
        
        builder.Property(e => e.Rating)
            .HasColumnName("rating");
        
        builder.Property(e => e.Comment)
            .HasColumnType("text")
            .HasColumnName("comment");
        
        builder.Property(e => e.ClientId)
            .HasColumnName("client_id");
        
        builder.Property(e => e.ProfessionalId)
            .HasColumnName("professional_id");
        
        builder.Property(e => e.ReviewDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("review_date");
        
        builder.HasOne(d => d.Client)
            .WithMany(p => p.ReviewClients)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("reviews_ibfk_2");
        
        builder.HasOne(d => d.Professional)
            .WithMany(p => p.ReviewProfessionals)
            .HasForeignKey(d => d.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("reviews_ibfk_3");
        
        builder.HasOne(d => d.Session)
            .WithMany(p => p.Reviews)
            .HasForeignKey(d => d.SessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("reviews_ibfk_1");
    }
}
