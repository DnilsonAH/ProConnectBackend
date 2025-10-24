using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Infrastructure.Entities;

public partial class ProfessionalProfile
{
    public uint ProfileId { get; set; }

    public uint UserId { get; set; }

    public uint SpecialtyId { get; set; }

    public string? Experience { get; set; }

    public string? Languages { get; set; }

    public decimal HourlyRate { get; set; }

    public string? ProfilePhotoUrl { get; set; }

    public string? Headline { get; set; }

    public virtual Specialty Specialty { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
