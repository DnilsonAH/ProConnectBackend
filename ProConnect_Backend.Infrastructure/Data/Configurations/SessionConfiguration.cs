using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToTable("sessions");
        
        builder.HasKey(e => e.SessionId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.SessionId)
            .HasColumnName("session_id");
        
        builder.Property(e => e.StartDate)
            .HasColumnType("timestamp")
            .HasColumnName("start_date");
        
        builder.Property(e => e.EndDate)
            .HasColumnType("timestamp")
            .HasColumnName("end_date");
        
        builder.Property(e => e.ProfessionalId)
            .HasColumnName("professional_id");
        
        builder.Property(e => e.ClientId)
            .HasColumnName("client_id");
        
        builder.Property(e => e.MeetUrl)
            .HasMaxLength(500)
            .HasColumnName("meet_url");
        
        builder.Property(e => e.Status)
            .HasMaxLength(50)
            .HasColumnName("status");
        
        builder.HasOne(d => d.Client)
            .WithMany(p => p.SessionClients)
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("sessions_ibfk_2");
        
        builder.HasOne(d => d.Professional)
            .WithMany(p => p.SessionProfessionals)
            .HasForeignKey(d => d.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("sessions_ibfk_1");
    }
}
