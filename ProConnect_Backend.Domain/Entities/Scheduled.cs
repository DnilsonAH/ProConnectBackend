using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class Scheduled
{
    public uint AvailabilityId { get; set; }

    public uint SessionId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public virtual Session Session { get; set; } = null!;
}
