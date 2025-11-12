using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class WeeklyAvailabilityConfiguration : IEntityTypeConfiguration<WeeklyAvailability>
{
    public void Configure(EntityTypeBuilder<WeeklyAvailability> builder)
    {
        builder.ToTable("weekly_availabilities");
        
        builder.HasKey(e => e.WeeklyAvailabilityId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.WeeklyAvailabilityId)
            .HasColumnName("weekly_availability_id");
        
        builder.Property(e => e.ProfessionalId)
            .HasColumnName("professional_id");
        
        builder.Property(e => e.WeekDay)
            .HasMaxLength(20)
            .HasColumnName("week_day");
        
        builder.Property(e => e.StartTime)
            .HasColumnType("time")
            .HasColumnName("start_time");
        
        builder.Property(e => e.EndTime)
            .HasColumnType("time")
            .HasColumnName("end_time");
        
        builder.Property(e => e.IsAvailable)
            .HasDefaultValue(true)
            .HasColumnName("is_available");
        
        builder.HasOne(d => d.Professional)
            .WithMany(p => p.WeeklyAvailabilities)
            .HasForeignKey(d => d.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("weekly_availabilities_ibfk_1");
    }
}
