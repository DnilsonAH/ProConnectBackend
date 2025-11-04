using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class WeeklyAvailability
{
    public uint WeeklyAvailabilityId { get; set; }

    public uint ProfessionalId { get; set; }

    public DateTime StartDateTime { get; set; }

    public DateTime EndDateTime { get; set; }

    public string WeekDay { get; set; } = null!;

    public virtual User Professional { get; set; } = null!;
}
