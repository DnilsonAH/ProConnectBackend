using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Infrastructure.Entities;

public partial class Specialty
{
    public uint SpecialtyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProfessionalProfile> ProfessionalProfiles { get; set; } = new List<ProfessionalProfile>();
}
