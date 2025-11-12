using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("professional_profiles")]
[Index("SpecializationId", Name = "professional_profiles_specialization_id_index")]
[Index("UserId", Name = "professional_profiles_user_id_index")]
public partial class ProfessionalProfile
{
    [Key]
    [Column("profile_id")]
    public uint ProfileId { get; set; }

    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("specialization_id")]
    public uint SpecializationId { get; set; }

    [Column("experience")]
    [StringLength(600)]
    public string? Experience { get; set; }

    [Column("presentation")]
    [StringLength(255)]
    public string? Presentation { get; set; }

    [ForeignKey("SpecializationId")]
    [InverseProperty("ProfessionalProfiles")]
    public virtual Specialization Specialization { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("ProfessionalProfiles")]
    public virtual User User { get; set; } = null!;
}
