using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class ScheduledConfiguration : IEntityTypeConfiguration<Scheduled>
{
    public void Configure(EntityTypeBuilder<Scheduled> builder)
    {
        builder.ToTable("scheduleds");
        
        builder.HasKey(e => e.AvailabilityId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.AvailabilityId)
            .HasColumnName("availability_id");
        
        builder.Property(e => e.SessionId)
            .HasColumnName("session_id");
        
        builder.Property(e => e.StartDate)
            .HasColumnType("timestamp")
            .HasColumnName("start_date");
        
        builder.Property(e => e.EndDate)
            .HasColumnType("timestamp")
            .HasColumnName("end_date");
        
        builder.HasOne(d => d.Session)
            .WithMany(p => p.Scheduleds)
            .HasForeignKey(d => d.SessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("scheduleds_ibfk_1");
    }
}
