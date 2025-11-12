using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class ProfessionalProfileConfiguration : IEntityTypeConfiguration<ProfessionalProfile>
{
    public void Configure(EntityTypeBuilder<ProfessionalProfile> builder)
    {
        builder.ToTable("professional_profiles");
        
        builder.HasKey(e => e.ProfileId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.ProfileId)
            .HasColumnName("profile_id");
        
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
        
        builder.Property(e => e.SpecializationId)
            .HasColumnName("specialization_id");
        
        builder.Property(e => e.Experience)
            .HasColumnType("text")
            .HasColumnName("experience");
        
        builder.Property(e => e.Presentation)
            .HasColumnType("text")
            .HasColumnName("presentation");
        
        builder.HasOne(d => d.Specialization)
            .WithMany(p => p.ProfessionalProfiles)
            .HasForeignKey(d => d.SpecializationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("professional_profiles_ibfk_2");
        
        builder.HasOne(d => d.User)
            .WithMany(p => p.ProfessionalProfiles)
            .HasForeignKey(d => d.UserId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("professional_profiles_ibfk_1");
    }
}
