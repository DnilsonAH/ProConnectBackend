using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");
        
        // Primary Key
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId)
            .HasColumnName("user_id");
        
        // Properties
        builder.Property(e => e.FirstName)
            .HasColumnName("first_name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(e => e.SecondName)
            .HasColumnName("second_name")
            .HasMaxLength(50);
        
        builder.Property(e => e.FirstSurname)
            .HasColumnName("first_surname")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(e => e.SecondSurname)
            .HasColumnName("second_surname")
            .HasMaxLength(50);
        
        builder.Property(e => e.Email)
            .HasColumnName("email")
            .IsRequired();
        
        builder.Property(e => e.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(255)
            .IsRequired();
        
        builder.Property(e => e.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(20);
        
        builder.Property(e => e.RegistrationDate)
            .HasColumnName("registration_date")
            .HasColumnType("datetime")
            .IsRequired();
        
        builder.Property(e => e.Role)
            .HasColumnName("role")
            .HasColumnType("enum('client','professional','admin')")
            .IsRequired();
        
        builder.Property(e => e.PhotoUrl)
            .HasColumnName("photo_url")
            .HasMaxLength(1000);
        
        // Indexes
        builder.HasIndex(e => e.Email, "users_email_unique")
            .IsUnique();
        
        builder.HasIndex(e => e.RegistrationDate, "users_registration_date_index");
        
        // Relationships
        builder.HasMany(e => e.JwtBlacklists)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.ProfessionalProfiles)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.ReviewClients)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.ReviewProfessionals)
            .WithOne(e => e.Professional)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.SessionClients)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.SessionProfessionals)
            .WithOne(e => e.Professional)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.Verifications)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(e => e.WeeklyAvailabilities)
            .WithOne(e => e.Professional)
            .HasForeignKey(e => e.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
