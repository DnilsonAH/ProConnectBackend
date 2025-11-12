using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class ProfessionCategory
{
    public uint CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public string? Description { get; set; }
    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}

