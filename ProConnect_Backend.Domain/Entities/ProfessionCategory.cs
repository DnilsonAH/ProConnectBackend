using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("profession_categories")]
public partial class ProfessionCategory
{
    [Key]
    [Column("category_id")]
    public uint CategoryId { get; set; }

    [Column("category_name")]
    [StringLength(255)]
    public string CategoryName { get; set; } = null!;

    [Column("description")]
    [StringLength(1000)]
    public string? Description { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
