using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("sessions")]
[Index("ClientId", Name = "sessions_client_id_foreign")]
[Index("ProfessionalId", Name = "sessions_professional_id_foreign")]
public partial class Session
{
    [Key]
    [Column("session_id")]
    public uint SessionId { get; set; }

    [Column("start_date", TypeName = "datetime")]
    public DateTime StartDate { get; set; }

    [Column("end_date", TypeName = "datetime")]
    public DateTime EndDate { get; set; }

    [Column("professional_id")]
    public uint ProfessionalId { get; set; }

    [Column("client_id")]
    public uint ClientId { get; set; }

    [Column("meet_url")]
    [StringLength(1000)]
    public string? MeetUrl { get; set; }

    [Column("status", TypeName = "enum('pending','confirmed','completed','cancelled')")]
    public string Status { get; set; } = null!;

    [ForeignKey("ClientId")]
    [InverseProperty("SessionClients")]
    public virtual User Client { get; set; } = null!;

    [InverseProperty("Session")]
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    [ForeignKey("ProfessionalId")]
    [InverseProperty("SessionProfessionals")]
    public virtual User Professional { get; set; } = null!;

    [InverseProperty("Session")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    [InverseProperty("Session")]
    public virtual ICollection<Scheduled> Scheduleds { get; set; } = new List<Scheduled>();
}
