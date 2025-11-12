using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class VerificationDocumentConfiguration : IEntityTypeConfiguration<VerificationDocument>
{
    public void Configure(EntityTypeBuilder<VerificationDocument> builder)
    {
        builder.ToTable("verification_documents");
        
        builder.HasKey(e => e.DocumentId)
            .HasName("PRIMARY");
        
        builder.Property(e => e.DocumentId)
            .HasColumnName("document_id");
        
        builder.Property(e => e.VerificationId)
            .HasColumnName("verification_id");
        
        builder.Property(e => e.DocumentType)
            .HasMaxLength(100)
            .HasColumnName("document_type");
        
        builder.Property(e => e.FileUrl)
            .HasMaxLength(500)
            .HasColumnName("file_url");
        
        builder.Property(e => e.UploadedAt)
            .HasColumnType("timestamp")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .HasColumnName("uploaded_at");
        
        builder.HasOne(d => d.Verification)
            .WithMany(p => p.VerificationDocuments)
            .HasForeignKey(d => d.VerificationId)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("verification_documents_ibfk_1");
    }
}
