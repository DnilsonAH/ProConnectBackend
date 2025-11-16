using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class ProfileSpecialization
{
    public uint ProfileSpecializationId { get; set; }

    public uint ProfileId { get; set; }

    public uint SpecializationId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ProfessionalProfile Profile { get; set; } = null!;

    public virtual Specialization Specialization { get; set; } = null!;
}
