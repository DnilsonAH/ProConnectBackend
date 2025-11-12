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
        // Configuraciones globales de MySQL
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        // Aplicar todas las configuraciones del assembly actual
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProConnectDbContext).Assembly);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
