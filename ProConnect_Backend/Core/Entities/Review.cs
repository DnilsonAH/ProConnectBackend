using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Core.Entities;

public partial class Review
{
    public uint ReviewId { get; set; }

    public uint ConsultationId { get; set; }

    public uint ClientId { get; set; }

    public uint ConsultantId { get; set; }

    public byte Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime ReviewDate { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual User Consultant { get; set; } = null!;

    public virtual Consultation Consultation { get; set; } = null!;
}
