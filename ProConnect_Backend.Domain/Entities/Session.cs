using System;
using System.Collections.Generic;
namespace ProConnect_Backend.Domain.Entities;

public partial class Session
{
    public uint SessionId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public uint ProfessionalId { get; set; }
    public uint ClientId { get; set; }
    public string? MeetUrl { get; set; }
    public string Status { get; set; } = null!;
    public virtual User Client { get; set; } = null!;
    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public virtual User Professional { get; set; } = null!;
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
    public virtual ICollection<Scheduled> Scheduleds { get; set; } = new List<Scheduled>();
}

