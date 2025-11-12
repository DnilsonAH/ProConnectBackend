using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class Review
{
    public uint ReviewId { get; set; }
    public uint SessionId { get; set; }
    public uint ClientId { get; set; }
    public uint ProfessionalId { get; set; }
    public byte Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime ReviewDate { get; set; }
    public virtual User Client { get; set; } = null!;
    public virtual User Professional { get; set; } = null!;
    public virtual Session Session { get; set; } = null!;
}

