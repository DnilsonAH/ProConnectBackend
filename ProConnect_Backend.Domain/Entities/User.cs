using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ProConnect_Backend.Domain.Entities;

[Table("users")]
[Index("Email", Name = "users_email_unique", IsUnique = true)]
[Index("RegistrationDate", Name = "users_registration_date_index")]
public partial class User
{
    [Key]
    [Column("user_id")]
    public uint UserId { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Column("second_name")]
    [StringLength(50)]
    public string? SecondName { get; set; }

    [Column("first_surname")]
    [StringLength(50)]
    public string FirstSurname { get; set; } = null!;

    [Column("second_surname")]
    [StringLength(50)]
    public string? SecondSurname { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    [Column("registration_date", TypeName = "datetime")]
    public DateTime RegistrationDate { get; set; }

    [Column("role", TypeName = "enum('client','professional','admin')")]
    public string Role { get; set; } = null!;

    [Column("photo_url")]
    [StringLength(1000)]
    public string? PhotoUrl { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<JwtBlacklist> JwtBlacklists { get; set; } = new List<JwtBlacklist>();

    [InverseProperty("User")]
    public virtual ICollection<ProfessionalProfile> ProfessionalProfiles { get; set; } = new List<ProfessionalProfile>();

    [InverseProperty("Client")]
    public virtual ICollection<Review> ReviewClients { get; set; } = new List<Review>();

    [InverseProperty("Professional")]
    public virtual ICollection<Review> ReviewProfessionals { get; set; } = new List<Review>();

    [InverseProperty("Client")]
    public virtual ICollection<Session> SessionClients { get; set; } = new List<Session>();

    [InverseProperty("Professional")]
    public virtual ICollection<Session> SessionProfessionals { get; set; } = new List<Session>();

    [InverseProperty("User")]
    public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();

    [InverseProperty("Professional")]
    public virtual ICollection<WeeklyAvailability> WeeklyAvailabilities { get; set; } = new List<WeeklyAvailability>();
}
