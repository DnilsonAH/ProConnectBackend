using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class Payment
{
    public uint PaymentId { get; set; }

    public uint ConsultationId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public string? Method { get; set; }

    public string? PaymentGatewayTransactionId { get; set; }

    public DateTime PaymentDate { get; set; }

    public virtual Consultation Consultation { get; set; } = null!;

    public virtual ICollection<Distribution> Distributions { get; set; } = new List<Distribution>();
}
