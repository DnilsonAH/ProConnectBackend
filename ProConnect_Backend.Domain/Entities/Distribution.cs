using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class Distribution
{
    public uint DistributionId { get; set; }

    public uint PaymentId { get; set; }

    public decimal ConsultantShare { get; set; }

    public decimal PlatformShare { get; set; }

    public string Status { get; set; } = null!;

    public DateTime? DistributionDate { get; set; }

    public virtual Payment Payment { get; set; } = null!;
}
