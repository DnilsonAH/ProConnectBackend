using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class Specialization
{
    public uint SpecializationId { get; set; }
    public uint ProfessionId { get; set; }
    public string SpecializationName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public virtual Profession Profession { get; set; } = null!;
    public virtual ICollection<ProfessionalProfile> ProfessionalProfiles { get; set; } = new List<ProfessionalProfile>();
}

