using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
{
    public void Configure(EntityTypeBuilder<Specialization> builder)
    {
        builder.ToTable("specializations");
        
        builder.HasKey(e => e.SpecializationId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.SpecializationId)
            .HasColumnName("specialization_id");
        
        builder.Property(e => e.ProfessionId)
            .HasColumnName("profession_id");
        
        builder.Property(e => e.SpecializationName)
            .HasMaxLength(150)
            .HasColumnName("specialization_name");
        
        builder.Property(e => e.Description)
            .HasColumnType("text")
            .HasColumnName("description");
        
        builder.HasOne(d => d.Profession)
            .WithMany(p => p.Specializations)
            .HasForeignKey(d => d.ProfessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("specializations_ibfk_1");
    }
}
