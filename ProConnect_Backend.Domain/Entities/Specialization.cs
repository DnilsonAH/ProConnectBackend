using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("specializations")]
[Index("ProfessionId", Name = "specializations_profession_id_foreign")]
public partial class Specialization
{
    [Key]
    [Column("specialization_id")]
    public uint SpecializationId { get; set; }

    [Column("profession_id")]
    public uint ProfessionId { get; set; }

    [Column("specialization_name")]
    [StringLength(255)]
    public string SpecializationName { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string Description { get; set; } = null!;

    [ForeignKey("ProfessionId")]
    [InverseProperty("Specializations")]
    public virtual Profession Profession { get; set; } = null!;

    [InverseProperty("Specialization")]
    public virtual ICollection<ProfessionalProfile> ProfessionalProfiles { get; set; } = new List<ProfessionalProfile>();
}
