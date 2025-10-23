using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Core.Entities;

public partial class Availability
{
    public uint AvailabilityId { get; set; }

    public uint ConsultantId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public bool? IsAvailable { get; set; }

    public virtual User Consultant { get; set; } = null!;
}
