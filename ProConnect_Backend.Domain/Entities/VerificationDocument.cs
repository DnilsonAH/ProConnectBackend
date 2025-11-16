using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class VerificationDocument
{
    public uint DocumentId { get; set; }

    public uint VerificationId { get; set; }

    public string DocumentType { get; set; } = null!;

    public string FileUrl { get; set; } = null!;

    public DateTime UploadedAt { get; set; }

    public virtual Verification Verification { get; set; } = null!;
}
