using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class ProfessionConfiguration : IEntityTypeConfiguration<Profession>
{
    public void Configure(EntityTypeBuilder<Profession> builder)
    {
        builder.ToTable("professions");
        
        builder.HasKey(e => e.ProfessionId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.ProfessionId)
            .HasColumnName("profession_id");
        
        builder.Property(e => e.CategoryId)
            .HasColumnName("category_id");
        
        builder.Property(e => e.ProfessionName)
            .HasMaxLength(100)
            .HasColumnName("profession_name");
        
        builder.Property(e => e.Description)
            .HasColumnType("text")
            .HasColumnName("description");
        
        builder.HasOne(d => d.Category)
            .WithMany(p => p.Professions)
            .HasForeignKey(d => d.CategoryId)
            .HasConstraintName("professions_ibfk_1");
    }
}
