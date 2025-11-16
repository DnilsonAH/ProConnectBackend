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

    public virtual DbSet<ProfileSpecialization> ProfileSpecializations { get; set; }

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
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<JwtBlacklist>(entity =>
        {
            entity.HasKey(e => e.JwtId).HasName("PRIMARY");

            entity.ToTable("jwt_blacklist");

            entity.HasIndex(e => e.UserId, "jwt_blacklist_user_id_foreign");

            entity.Property(e => e.JwtId).HasColumnName("jwt_id");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.Token)
                .HasMaxLength(1000)
                .HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.JwtBlacklists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jwt_blacklist_user_id_foreign");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PRIMARY");

            entity.ToTable("payments");

            entity.HasIndex(e => e.PaymentDate, "payments_payment_date_index");

            entity.HasIndex(e => e.SessionId, "payments_session_id_index");

            entity.HasIndex(e => e.Status, "payments_status_index");

            entity.HasIndex(e => new { e.Status, e.PaymentDate }, "payments_status_payment_date_index");

            entity.Property(e => e.PaymentId).HasColumnName("payment_id");
            entity.Property(e => e.Method)
                .HasMaxLength(50)
                .HasColumnName("method");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'")
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(10, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.Session).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("payments_session_id_foreign");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.ProfessionId).HasName("PRIMARY");

            entity.ToTable("professions");

            entity.HasIndex(e => e.CategoryId, "professions_category_id_foreign");

            entity.Property(e => e.ProfessionId).HasColumnName("profession_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.ProfessionName)
                .HasMaxLength(255)
                .HasColumnName("profession_name");

            entity.HasOne(d => d.Category).WithMany(p => p.Professions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professions_category_id_foreign");
        });

        modelBuilder.Entity<ProfessionCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("profession_categories");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(255)
                .HasColumnName("category_name");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
        });

        modelBuilder.Entity<ProfessionalProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.ToTable("professional_profiles");

            entity.HasIndex(e => e.UserId, "professional_profiles_user_id_unique").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Experience)
                .HasMaxLength(600)
                .HasColumnName("experience");
            entity.Property(e => e.Presentation)
                .HasMaxLength(255)
                .HasColumnName("presentation");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithOne(p => p.ProfessionalProfile)
                .HasForeignKey<ProfessionalProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_profiles_user_id_foreign");
        });

        modelBuilder.Entity<ProfileSpecialization>(entity =>
        {
            entity.HasKey(e => e.ProfileSpecializationId).HasName("PRIMARY");

            entity.ToTable("profile_specializations");

            entity.HasIndex(e => e.ProfileId, "profile_specializations_profile_id_index");

            entity.HasIndex(e => new { e.ProfileId, e.SpecializationId }, "profile_specializations_profile_specialization_unique").IsUnique();

            entity.HasIndex(e => e.SpecializationId, "profile_specializations_specialization_id_index");

            entity.Property(e => e.ProfileSpecializationId).HasColumnName("profile_specialization_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfileSpecializations)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profile_specializations_profile_id_foreign");

            entity.HasOne(d => d.Specialization).WithMany(p => p.ProfileSpecializations)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("profile_specializations_specialization_id_foreign");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.ToTable("reviews");

            entity.HasIndex(e => e.ClientId, "reviews_client_id_index");

            entity.HasIndex(e => e.ProfessionalId, "reviews_professional_id_index");

            entity.HasIndex(e => e.Rating, "reviews_rating_index");

            entity.HasIndex(e => e.ReviewDate, "reviews_review_date_index");

            entity.HasIndex(e => e.SessionId, "reviews_session_id_foreign");

            entity.Property(e => e.ReviewId).HasColumnName("review_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("review_date");
            entity.Property(e => e.SessionId).HasColumnName("session_id");

            entity.HasOne(d => d.Client).WithMany(p => p.ReviewClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_client_id_foreign");

            entity.HasOne(d => d.Professional).WithMany(p => p.ReviewProfessionals)
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_professional_id_foreign");

            entity.HasOne(d => d.Session).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("reviews_session_id_foreign");
        });

        modelBuilder.Entity<Scheduled>(entity =>
        {
            entity.HasKey(e => e.AvailabilityId).HasName("PRIMARY");

            entity.ToTable("scheduleds");

            entity.HasIndex(e => e.SessionId, "scheduleds_session_id_index");

            entity.HasIndex(e => e.StartDate, "scheduleds_start_date_index");

            entity.Property(e => e.AvailabilityId).HasColumnName("availability_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");

            entity.HasOne(d => d.Session).WithMany(p => p.Scheduleds)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("scheduleds_session_id_foreign");
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("PRIMARY");

            entity.ToTable("sessions");

            entity.HasIndex(e => e.ClientId, "sessions_client_id_index");

            entity.HasIndex(e => e.ProfessionalId, "sessions_professional_id_index");

            entity.HasIndex(e => e.StartDate, "sessions_start_date_index");

            entity.HasIndex(e => e.Status, "sessions_status_index");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.ClientId).HasColumnName("client_id");
            entity.Property(e => e.EndDate)
                .HasColumnType("datetime")
                .HasColumnName("end_date");
            entity.Property(e => e.MeetUrl)
                .HasMaxLength(1000)
                .HasColumnName("meet_url");
            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id");
            entity.Property(e => e.StartDate)
                .HasColumnType("datetime")
                .HasColumnName("start_date");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'")
                .HasColumnType("enum('pending','confirmed','completed','cancelled')")
                .HasColumnName("status");

            entity.HasOne(d => d.Client).WithMany(p => p.SessionClients)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_client_id_foreign");

            entity.HasOne(d => d.Professional).WithMany(p => p.SessionProfessionals)
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("sessions_professional_id_foreign");
        });

        modelBuilder.Entity<Specialization>(entity =>
        {
            entity.HasKey(e => e.SpecializationId).HasName("PRIMARY");

            entity.ToTable("specializations");

            entity.HasIndex(e => e.ProfessionId, "specializations_profession_id_foreign");

            entity.Property(e => e.SpecializationId).HasColumnName("specialization_id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.ProfessionId).HasColumnName("profession_id");
            entity.Property(e => e.SpecializationName)
                .HasMaxLength(255)
                .HasColumnName("specialization_name");

            entity.HasOne(d => d.Profession).WithMany(p => p.Specializations)
                .HasForeignKey(d => d.ProfessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("specializations_profession_id_foreign");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.HasIndex(e => e.RegistrationDate, "users_registration_date_index");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name");
            entity.Property(e => e.FirstSurname)
                .HasMaxLength(50)
                .HasColumnName("first_surname");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.PhotoUrl)
                .HasMaxLength(1000)
                .HasColumnName("photo_url");
            entity.Property(e => e.RegistrationDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("registration_date");
            entity.Property(e => e.Role)
                .HasColumnType("enum('client','professional','admin')")
                .HasColumnName("role");
            entity.Property(e => e.SecondName)
                .HasMaxLength(50)
                .HasColumnName("second_name");
            entity.Property(e => e.SecondSurname)
                .HasMaxLength(50)
                .HasColumnName("second_surname");
        });

        modelBuilder.Entity<Verification>(entity =>
        {
            entity.HasKey(e => e.VerificationId).HasName("PRIMARY");

            entity.ToTable("verifications");

            entity.HasIndex(e => e.CreatedAt, "verifications_created_at_index");

            entity.HasIndex(e => e.Status, "verifications_status_index");

            entity.HasIndex(e => e.UserId, "verifications_user_id_index");

            entity.HasIndex(e => e.VerificationDate, "verifications_verification_date_index");

            entity.Property(e => e.VerificationId).HasColumnName("verification_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Notes)
                .HasMaxLength(500)
                .HasColumnName("notes");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Pending'")
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.VerificationDate)
                .HasColumnType("datetime")
                .HasColumnName("verification_date");

            entity.HasOne(d => d.User).WithMany(p => p.Verifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("verifications_user_id_foreign");
        });

        modelBuilder.Entity<VerificationDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PRIMARY");

            entity.ToTable("verification_documents");

            entity.HasIndex(e => e.DocumentType, "verification_documents_document_type_index");

            entity.HasIndex(e => e.UploadedAt, "verification_documents_uploaded_at_index");

            entity.HasIndex(e => e.VerificationId, "verification_documents_verification_id_index");

            entity.Property(e => e.DocumentId).HasColumnName("document_id");
            entity.Property(e => e.DocumentType)
                .HasMaxLength(50)
                .HasColumnName("document_type");
            entity.Property(e => e.FileUrl)
                .HasMaxLength(500)
                .HasColumnName("file_url");
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("uploaded_at");
            entity.Property(e => e.VerificationId).HasColumnName("verification_id");

            entity.HasOne(d => d.Verification).WithMany(p => p.VerificationDocuments)
                .HasForeignKey(d => d.VerificationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("verification_documents_verification_id_foreign");
        });

        modelBuilder.Entity<WeeklyAvailability>(entity =>
        {
            entity.HasKey(e => e.WeeklyAvailabilityId).HasName("PRIMARY");

            entity.ToTable("weekly_availabilities");

            entity.HasIndex(e => e.ProfessionalId, "weekly_availabilities_professional_id_index");

            entity.HasIndex(e => new { e.ProfessionalId, e.WeekDay }, "weekly_availabilities_professional_id_week_day_index");

            entity.Property(e => e.WeeklyAvailabilityId).HasColumnName("weekly_availability_id");
            entity.Property(e => e.EndTime)
                .HasColumnType("time")
                .HasColumnName("end_time");
            entity.Property(e => e.IsAvailable)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_available");
            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id");
            entity.Property(e => e.StartTime)
                .HasColumnType("time")
                .HasColumnName("start_time");
            entity.Property(e => e.WeekDay)
                .HasColumnType("enum('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday')")
                .HasColumnName("week_day");

            entity.HasOne(d => d.Professional).WithMany(p => p.WeeklyAvailabilities)
                .HasForeignKey(d => d.ProfessionalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("weekly_availabilities_professional_id_foreign");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
