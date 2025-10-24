using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Infrastructure.Entities;

namespace ProConnect_Backend.Infrastructure.Data;

public partial class ProConnectDbContext : DbContext
{
    public ProConnectDbContext(DbContextOptions<ProConnectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Availability> Availabilities { get; set; }

    public virtual DbSet<Consultation> Consultations { get; set; }

    public virtual DbSet<Distribution> Distributions { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ProfessionalPaymentInfo> ProfessionalPaymentInfos { get; set; }

    public virtual DbSet<ProfessionalProfile> ProfessionalProfiles { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

    public virtual DbSet<VerificationDocument> VerificationDocuments { get; set; }

    public virtual DbSet<VideoCall> VideoCalls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Availability>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PRIMARY");

            entity.ToTable("Availability");

            entity.HasIndex(e => e.ConsultantId, "ConsultantId");

            entity.Property(e => e.EndDateTime).HasColumnType("datetime");
            entity.Property(e => e.IsAvailable)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.StartDateTime).HasColumnType("datetime");

            entity.HasOne(d => d.Consultant).WithMany(p => p.Availabilities)
                .HasForeignKey(d => d.ConsultantId)
                .HasConstraintName("Availability_ibfk_1");
        });

        modelBuilder.Entity<Consultation>(entity =>
        {
            entity.HasKey(e => e.ConsultationId).HasName("PRIMARY");

            entity.HasIndex(e => e.ClientId, "ClientId");

            entity.HasIndex(e => e.ConsultantId, "ConsultantId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.DurationMinutes).HasDefaultValueSql("'60'");
            entity.Property(e => e.ScheduledDateTime).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Scheduled'")
                .HasColumnType("enum('Scheduled','Completed','CancelledByClient','CancelledByConsultant')");

            entity.HasOne(d => d.Client).WithMany(p => p.ConsultationClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Consultations_ibfk_1");

            entity.HasOne(d => d.Consultant).WithMany(p => p.ConsultationConsultants)
                .HasForeignKey(d => d.ConsultantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Consultations_ibfk_2");
        });

        modelBuilder.Entity<Distribution>(entity =>
        {
            entity.HasKey(e => e.DistributionId).HasName("PRIMARY");

            entity.HasIndex(e => e.PaymentId, "PaymentId");

            entity.Property(e => e.ConsultantShare).HasPrecision(10, 2);
            entity.Property(e => e.DistributionDate).HasColumnType("timestamp");
            entity.Property(e => e.PlatformShare).HasPrecision(10, 2);
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Paid')");

            entity.HasOne(d => d.Payment).WithMany(p => p.Distributions)
                .HasForeignKey(d => d.PaymentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Distributions_ibfk_1");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Message).HasColumnType("text");
            entity.Property(e => e.SentAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Notifications_ibfk_1");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.HasIndex(e => e.ConsultationId, "ConsultationId");

            entity.Property(e => e.Method).HasMaxLength(50);
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.PaymentGatewayTransactionId).HasMaxLength(255);
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Succeeded','Failed')");
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2);

            entity.HasOne(d => d.Consultation).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Payments_ibfk_1");
        });

        modelBuilder.Entity<ProfessionalPaymentInfo>(entity =>
        {
            entity.HasKey(e => e.PaymentInfoId).HasName("PRIMARY");

            entity.ToTable("ProfessionalPaymentInfo");

            entity.HasIndex(e => e.UserId, "UserId").IsUnique();

            entity.Property(e => e.Details).HasColumnType("text");
            entity.Property(e => e.PaymentType).HasColumnType("enum('BankAccount','Wallet')");

            entity.HasOne(d => d.User).WithOne(p => p.ProfessionalPaymentInfo)
                .HasForeignKey<ProfessionalPaymentInfo>(d => d.UserId)
                .HasConstraintName("ProfessionalPaymentInfo_ibfk_1");
        });

        modelBuilder.Entity<ProfessionalProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.HasIndex(e => e.SpecialtyId, "SpecialtyId");

            entity.HasIndex(e => e.UserId, "UserId").IsUnique();

            entity.Property(e => e.Experience).HasColumnType("text");
            entity.Property(e => e.Headline).HasMaxLength(255);
            entity.Property(e => e.HourlyRate).HasPrecision(10, 2);
            entity.Property(e => e.Languages).HasMaxLength(255);
            entity.Property(e => e.ProfilePhotoUrl)
                .HasMaxLength(255)
                .HasColumnName("ProfilePhotoURL");

            entity.HasOne(d => d.Specialty).WithMany(p => p.ProfessionalProfiles)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProfessionalProfiles_ibfk_2");

            entity.HasOne(d => d.User).WithOne(p => p.ProfessionalProfile)
                .HasForeignKey<ProfessionalProfile>(d => d.UserId)
                .HasConstraintName("ProfessionalProfiles_ibfk_1");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.HasIndex(e => e.ClientId, "ClientId");

            entity.HasIndex(e => e.ConsultantId, "ConsultantId");

            entity.HasIndex(e => e.ConsultationId, "ConsultationId").IsUnique();

            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Client).WithMany(p => p.ReviewClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reviews_ibfk_2");

            entity.HasOne(d => d.Consultant).WithMany(p => p.ReviewConsultants)
                .HasForeignKey(d => d.ConsultantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reviews_ibfk_3");

            entity.HasOne(d => d.Consultation).WithOne(p => p.Review)
                .HasForeignKey<Review>(d => d.ConsultationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Reviews_ibfk_1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.HasIndex(e => e.RoleName, "RoleName").IsUnique();

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PRIMARY");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PRIMARY");

            entity.HasIndex(e => e.RoleId, "RoleId");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "UK_User_Role").IsUnique();

            entity.Property(e => e.AssignedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("UserRoles_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserRoles_ibfk_1");
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'Pending'")
                .HasColumnType("enum('Pending','Verified','Rejected')");
            entity.Property(e => e.VerificationDate).HasColumnType("timestamp");

            entity.HasOne(d => d.User).WithMany(p => p.Verifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Verifications_ibfk_1");
        });

        modelBuilder.Entity<VerificationDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PRIMARY");

            entity.HasIndex(e => e.VerificationId, "VerificationId");

            entity.Property(e => e.DocumentType).HasColumnType("enum('DNI','Title','License','Other')");
            entity.Property(e => e.FileUrl).HasMaxLength(255);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");

            entity.HasOne(d => d.Verification).WithMany(p => p.VerificationDocuments)
                .HasForeignKey(d => d.VerificationId)
                .HasConstraintName("VerificationDocuments_ibfk_1");
        });

        modelBuilder.Entity<VideoCall>(entity =>
        {
            entity.HasKey(e => e.VideoCallId).HasName("PRIMARY");

            entity.HasIndex(e => e.ConsultationId, "ConsultationId").IsUnique();

            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.RoomUrl)
                .HasMaxLength(255)
                .HasColumnName("RoomURL");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Consultation).WithOne(p => p.VideoCall)
                .HasForeignKey<VideoCall>(d => d.ConsultationId)
                .HasConstraintName("VideoCalls_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
