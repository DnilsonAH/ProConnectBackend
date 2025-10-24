using System;
using System.Collections.Generic;

namespace ProConnect_Backend.Infrastructure.Entities;

public partial class User
{
    public uint UserId { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime RegistrationDate { get; set; }

    public virtual ICollection<Availability> Availabilities { get; set; } = new List<Availability>();

    public virtual ICollection<Consultation> ConsultationClients { get; set; } = new List<Consultation>();

    public virtual ICollection<Consultation> ConsultationConsultants { get; set; } = new List<Consultation>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ProfessionalPaymentInfo? ProfessionalPaymentInfo { get; set; }

    public virtual ProfessionalProfile? ProfessionalProfile { get; set; }

    public virtual ICollection<Review> ReviewClients { get; set; } = new List<Review>();

    public virtual ICollection<Review> ReviewConsultants { get; set; } = new List<Review>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();
}
