using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("verification_documents")]
[Index("DocumentType", Name = "verification_documents_document_type_index")]
[Index("UploadedAt", Name = "verification_documents_uploaded_at_index")]
[Index("VerificationId", Name = "verification_documents_verification_id_index")]
public partial class VerificationDocument
{
    [Key]
    [Column("document_id")]
    public uint DocumentId { get; set; }

    [Column("verification_id")]
    public uint VerificationId { get; set; }

    [Column("document_type")]
    [StringLength(50)]
    public string DocumentType { get; set; } = null!;

    [Column("file_url")]
    [StringLength(500)]
    public string FileUrl { get; set; } = null!;

    [Column("uploaded_at", TypeName = "datetime")]
    public DateTime UploadedAt { get; set; }

    [ForeignKey("VerificationId")]
    [InverseProperty("VerificationDocuments")]
    public virtual Verification Verification { get; set; } = null!;
}
