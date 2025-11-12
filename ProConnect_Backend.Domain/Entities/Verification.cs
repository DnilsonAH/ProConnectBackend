using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class Verification
{
    public uint VerificationId { get; set; }
    public uint UserId { get; set; }
    public string Status { get; set; } = null!;
    public DateTime? VerificationDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual User User { get; set; } = null!;
    public virtual ICollection<VerificationDocument> VerificationDocuments { get; set; } = new List<VerificationDocument>();
}

