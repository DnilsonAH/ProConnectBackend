using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");
        
        builder.HasKey(e => e.PaymentId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.PaymentId)
            .HasColumnName("payment_id");
        
        builder.Property(e => e.SessionId)
            .HasColumnName("session_id");
        
        builder.Property(e => e.TotalAmount)
            .HasPrecision(10, 2)
            .HasColumnName("total_amount");
        
        builder.Property(e => e.Status)
            .HasMaxLength(50)
            .HasColumnName("status");
        
        builder.Property(e => e.Method)
            .HasMaxLength(50)
            .HasColumnName("method");
        
        builder.Property(e => e.PaymentDate)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("payment_date");
        
        builder.HasOne(d => d.Session)
            .WithMany(p => p.Payments)
            .HasForeignKey(d => d.SessionId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("payments_ibfk_1");
    }
}
