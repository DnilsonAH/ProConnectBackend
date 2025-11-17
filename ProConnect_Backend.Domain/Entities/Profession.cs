using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class Profession
{
    public uint ProfessionId { get; set; }

    public uint CategoryId { get; set; }

    public string ProfessionName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ProfessionCategory Category { get; set; } = null!;

    public virtual ICollection<Specialization> Specializations { get; set; } = new List<Specialization>();
}
