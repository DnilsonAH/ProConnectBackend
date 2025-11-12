using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("reviews")]
[Index("ClientId", Name = "reviews_client_id_index")]
[Index("ProfessionalId", Name = "reviews_professional_id_index")]
[Index("Rating", Name = "reviews_rating_index")]
[Index("ReviewDate", Name = "reviews_review_date_index")]
[Index("SessionId", Name = "reviews_session_id_foreign")]
public partial class Review
{
    [Key]
    [Column("review_id")]
    public uint ReviewId { get; set; }

    [Column("session_id")]
    public uint SessionId { get; set; }

    [Column("client_id")]
    public uint ClientId { get; set; }

    [Column("professional_id")]
    public uint ProfessionalId { get; set; }

    [Column("rating")]
    public byte Rating { get; set; }

    [Column("comment")]
    [StringLength(255)]
    public string? Comment { get; set; }

    [Column("review_date", TypeName = "datetime")]
    public DateTime ReviewDate { get; set; }

    [ForeignKey("ClientId")]
    [InverseProperty("ReviewClients")]
    public virtual User Client { get; set; } = null!;

    [ForeignKey("ProfessionalId")]
    [InverseProperty("ReviewProfessionals")]
    public virtual User Professional { get; set; } = null!;

    [ForeignKey("SessionId")]
    [InverseProperty("Reviews")]
    public virtual Session Session { get; set; } = null!;
}
