using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class ProfessionalProfile
{
    public uint ProfileId { get; set; }

    public uint UserId { get; set; }

    public string? Experience { get; set; }

    public string? Presentation { get; set; }

    public virtual ICollection<ProfileSpecialization> ProfileSpecializations { get; set; } = new List<ProfileSpecialization>();

    public virtual User User { get; set; } = null!;
}
