using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("verifications")]
[Index("CreatedAt", Name = "verifications_created_at_index")]
[Index("Status", Name = "verifications_status_index")]
[Index("UserId", Name = "verifications_user_id_index")]
[Index("VerificationDate", Name = "verifications_verification_date_index")]
public partial class Verification
{
    [Key]
    [Column("verification_id")]
    public uint VerificationId { get; set; }

    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("status")]
    [StringLength(50)]
    public string Status { get; set; } = null!;

    [Column("verification_date", TypeName = "datetime")]
    public DateTime? VerificationDate { get; set; }

    [Column("notes")]
    [StringLength(500)]
    public string? Notes { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Verifications")]
    public virtual User User { get; set; } = null!;

    [InverseProperty("Verification")]
    public virtual ICollection<VerificationDocument> VerificationDocuments { get; set; } = new List<VerificationDocument>();
}
