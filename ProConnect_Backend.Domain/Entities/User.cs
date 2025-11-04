using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Domain.Entities;

public partial class User
{
    public uint UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime RegistrationDate { get; set; }

    public string Role { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public virtual ProfessionalProfile? ProfessionalProfile { get; set; }

    public virtual ICollection<Review> ReviewClients { get; set; } = new List<Review>();

    public virtual ICollection<Review> ReviewProfessionals { get; set; } = new List<Review>();

    public virtual ICollection<Session> SessionClients { get; set; } = new List<Session>();

    public virtual ICollection<Session> SessionProfessionals { get; set; } = new List<Session>();

    public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();

    public virtual ICollection<WeeklyAvailability> WeeklyAvailabilities { get; set; } = new List<WeeklyAvailability>();
}
