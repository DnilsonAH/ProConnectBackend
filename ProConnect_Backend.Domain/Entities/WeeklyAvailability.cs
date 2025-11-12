using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class WeeklyAvailability
{
    public uint WeeklyAvailabilityId { get; set; }
    public uint ProfessionalId { get; set; }
    public string WeekDay { get; set; } = null!;
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool? IsAvailable { get; set; }
    
    // Navigation properties
    public virtual User Professional { get; set; } = null!;
}

