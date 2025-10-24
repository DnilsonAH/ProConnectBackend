using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Infrastructure.Entities;

public partial class Consultation
{
    public uint ConsultationId { get; set; }

    public uint ClientId { get; set; }

    public uint ConsultantId { get; set; }

    public DateTime ScheduledDateTime { get; set; }

    public int DurationMinutes { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual User Client { get; set; } = null!;

    public virtual User Consultant { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Review? Review { get; set; }

    public virtual VideoCall? VideoCall { get; set; }
}
