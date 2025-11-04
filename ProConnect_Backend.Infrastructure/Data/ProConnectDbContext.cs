using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProConnect_Backend.Domain.Entities;

namespace ProConnect_Backend.Infrastructure.Data;

public partial class ProConnectDbContext : DbContext
{
    public ProConnectDbContext()
    {
    }

    public ProConnectDbContext(DbContextOptions<ProConnectDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<ProfessionalProfile> ProfessionalProfiles { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Scheduled> Scheduleds { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Specialty> Specialties { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Verification> Verifications { get; set; }

    public virtual DbSet<VerificationDocument> VerificationDocuments { get; set; }

    public virtual DbSet<WeeklyAvailability> WeeklyAvailabilities { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=34.95.233.65;port=3306;database=Proconnect;uid=TestDBAcc;pwd=c1bdb203:Csdcs", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.41-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

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

        modelBuilder.Entity<ProfessionalProfile>(entity =>
        {
            entity.HasKey(e => e.ProfileId).HasName("PRIMARY");

            entity.ToTable("professional_profiles");

            entity.HasIndex(e => e.SpecialtyId, "professional_profiles_specialty_id_index");

            entity.HasIndex(e => e.UserId, "professional_profiles_user_id_unique").IsUnique();

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");
            entity.Property(e => e.Experience)
                .HasMaxLength(600)
                .HasColumnName("experience");
            entity.Property(e => e.Headline)
                .HasMaxLength(255)
                .HasColumnName("headline");
            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Specialty).WithMany(p => p.ProfessionalProfiles)
                .HasForeignKey(d => d.SpecialtyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_profiles_specialty_id_foreign");

            entity.HasOne(d => d.User).WithOne(p => p.ProfessionalProfile)
                .HasForeignKey<ProfessionalProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("professional_profiles_user_id_foreign");
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
            entity.Property(e => e.Rating)
                .HasPrecision(2, 2)
                .HasColumnName("rating");
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

            entity.HasIndex(e => e.SessionId, "scheduleds_session_id_foreign");

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

            entity.HasIndex(e => e.ClientId, "sessions_client_id_foreign");

            entity.HasIndex(e => e.ProfessionalId, "sessions_professional_id_foreign");

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
                .HasMaxLength(255)
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

        modelBuilder.Entity<Specialty>(entity =>
        {
            entity.HasKey(e => e.SpecialtyId).HasName("PRIMARY");

            entity.ToTable("specialties");

            entity.HasIndex(e => e.Name, "specialties_name_unique").IsUnique();

            entity.Property(e => e.SpecialtyId).HasColumnName("specialty_id");
            entity.Property(e => e.Description)
                .HasMaxLength(600)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_unique").IsUnique();

            entity.HasIndex(e => e.RegistrationDate, "users_registration_date_index");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
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
                .HasMaxLength(255)
                .HasColumnName("role");
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

            entity.HasIndex(e => new { e.ProfessionalId, e.StartDateTime, e.EndDateTime }, "professional_id_start_date_time_end_date_time_index");

            entity.HasIndex(e => e.EndDateTime, "weekly_availabilities_end_date_time_index");

            entity.HasIndex(e => e.ProfessionalId, "weekly_availabilities_professional_id_index");

            entity.HasIndex(e => e.StartDateTime, "weekly_availabilities_start_date_time_index");

            entity.HasIndex(e => e.WeekDay, "weekly_availabilities_week_day_index");

            entity.Property(e => e.WeeklyAvailabilityId).HasColumnName("weekly_availability_id");
            entity.Property(e => e.EndDateTime)
                .HasColumnType("datetime")
                .HasColumnName("end_date_time");
            entity.Property(e => e.ProfessionalId).HasColumnName("professional_id");
            entity.Property(e => e.StartDateTime)
                .HasColumnType("datetime")
                .HasColumnName("start_date_time");
            entity.Property(e => e.WeekDay)
                .HasDefaultValueSql("'DEFAULT TRUE'")
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
