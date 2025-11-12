using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class ProfessionCategoryConfiguration : IEntityTypeConfiguration<ProfessionCategory>
{
    public void Configure(EntityTypeBuilder<ProfessionCategory> builder)
    {
        builder.ToTable("profession_categories");
        
        builder.HasKey(e => e.CategoryId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.CategoryId)
            .HasColumnName("category_id");
        
        builder.Property(e => e.CategoryName)
            .HasMaxLength(100)
            .HasColumnName("category_name");
        
        builder.Property(e => e.Description)
            .HasColumnType("text")
            .HasColumnName("description");
    }
}
