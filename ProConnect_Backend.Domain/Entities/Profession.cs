using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("professions")]
[Index("CategoryId", Name = "professions_category_id_foreign")]
public partial class Profession
{
    [Key]
    [Column("profession_id")]
    public uint ProfessionId { get; set; }

    [Column("category_id")]
    public uint CategoryId { get; set; }

    [Column("profession_name")]
    [StringLength(255)]
    public string ProfessionName { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Professions")]
    public virtual ProfessionCategory Category { get; set; } = null!;

    [InverseProperty("Profession")]
    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}
