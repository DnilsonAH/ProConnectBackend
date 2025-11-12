namespace ProConnect_Backend.Domain.Entities;

public partial class User
{
    public uint UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string? SecondName { get; set; }
    public string FirstSurname { get; set; } = null!;
    public string? SecondSurname { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Role { get; set; } = null!;
    public string? PhotoUrl { get; set; }

    // Navigation properties
    public virtual ICollection<JwtBlacklist> JwtBlacklists { get; set; } = new List<JwtBlacklist>();
    public virtual ICollection<ProfessionalProfile> ProfessionalProfiles { get; set; } = new List<ProfessionalProfile>();
    public virtual ICollection<Review> ReviewClients { get; set; } = new List<Review>();
    public virtual ICollection<Review> ReviewProfessionals { get; set; } = new List<Review>();
    public virtual ICollection<Session> SessionClients { get; set; } = new List<Session>();
    public virtual ICollection<Session> SessionProfessionals { get; set; } = new List<Session>();
    public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();
    public virtual ICollection<WeeklyAvailability> WeeklyAvailabilities { get; set; } = new List<WeeklyAvailability>();
}

