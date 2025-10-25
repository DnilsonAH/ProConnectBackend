using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class ProfessionalPaymentInfo
{
    public uint PaymentInfoId { get; set; }

    public uint UserId { get; set; }

    public string PaymentType { get; set; } = null!;

    public string Details { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
