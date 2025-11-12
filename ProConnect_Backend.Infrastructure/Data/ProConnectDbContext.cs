using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data;

public partial class ProConnectDbContext : DbContext
{
    public ProConnectDbContext(DbContextOptions<ProConnectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<JwtBlacklist> JwtBlacklists { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<ProfessionCategory> ProfessionCategories { get; set; }

    public virtual DbSet<ProfessionalProfile> ProfessionalProfiles { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Scheduled> Scheduleds { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Specialization> Specializations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

    public virtual DbSet<VerificationDocument> VerificationDocuments { get; set; }

    public virtual DbSet<WeeklyAvailability> WeeklyAvailabilities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<JwtBlacklist>(entity =>
        {
            entity.HasKey(e => e.JwtId).HasName("PRIMARY");

            entity.HasOne(d => d.User).WithMany(p => p.JwtBlacklists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jwt_blacklist_user_id_foreign");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.Property(e => e.PaymentDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Status).HasDefaultValueSql("'Pending'");

            entity.HasOne(d => d.Session).WithMany(p => p.Payments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_session_id_foreign");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionId).HasName("PRIMARY");

            entity.HasOne(d => d.Category).WithMany(p => p.Professions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professions_category_id_foreign");
        });

        modelBuilder.Entity<ProfessionCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");
        });

        modelBuilder.Entity<ProfessionalProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.HasOne(d => d.Specialization).WithMany(p => p.ProfessionalProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_profiles_specialization_id_foreign");

            entity.HasOne(d => d.User).WithMany(p => p.ProfessionalProfiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_profiles_user_id_foreign");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.Property(e => e.ReviewDate).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Client).WithMany(p => p.ReviewClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_client_id_foreign");

            entity.HasOne(d => d.Professional).WithMany(p => p.ReviewProfessionals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_professional_id_foreign");

            entity.HasOne(d => d.Session).WithMany(p => p.Reviews)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_session_id_foreign");
        });

        modelBuilder.Entity<Scheduled>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PRIMARY");

            entity.HasOne(d => d.Session).WithMany(p => p.Scheduleds)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("scheduleds_session_id_foreign");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PRIMARY");

            entity.Property(e => e.Status).HasDefaultValueSql("'pending'");

            entity.HasOne(d => d.Client).WithMany(p => p.SessionClients)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_client_id_foreign");

            entity.HasOne(d => d.Professional).WithMany(p => p.SessionProfessionals)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_professional_id_foreign");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.SpecializationId).HasName("PRIMARY");

            entity.HasOne(d => d.Profession).WithMany(p => p.Specializations)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specializations_profession_id_foreign");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.Property(e => e.RegistrationDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PRIMARY");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Status).HasDefaultValueSql("'Pending'");

            entity.HasOne(d => d.User).WithMany(p => p.Verifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("verifications_user_id_foreign");
        });

        modelBuilder.Entity<VerificationDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PRIMARY");

            entity.Property(e => e.UploadedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Verification).WithMany(p => p.VerificationDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("verification_documents_verification_id_foreign");
        });

        modelBuilder.Entity<WeeklyAvailability>(entity =>
        {
            entity.HasKey(e => e.WeeklyAvailabilityId).HasName("PRIMARY");

            entity.Property(e => e.IsAvailable).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Professional).WithMany(p => p.WeeklyAvailabilities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("weekly_availabilities_professional_id_foreign");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
