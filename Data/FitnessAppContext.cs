using System;
using System.Collections.Generic;
using fitapp.Models;
using fitapp.Models.fitapp.Models;
using Microsoft.EntityFrameworkCore;

namespace fitapp.Data;

public partial class FitnessAppContext : DbContext
{
    public FitnessAppContext()
    {
    }

    public FitnessAppContext(DbContextOptions<FitnessAppContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DietPlan> DietPlans { get; set; }

    public virtual DbSet<Gamification> Gamifications { get; set; }

    public virtual DbSet<ProgressTracking> ProgressTrackings { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Call> Calls { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public virtual DbSet<Voice> Voices { get; set; }

    public virtual DbSet<VrModule> VrModules { get; set; }

    public virtual DbSet<Workout> Workouts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-0LRF037;Database=FitnessApp;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DietPlan>(entity =>
        {
            entity.HasKey(e => e.DietPlanId).HasName("PK__DietPlan__D256E16AB6669391");

            entity.Property(e => e.DietPlanId).HasColumnName("DietPlanID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Gamification>(entity =>
        {
            entity.HasKey(e => e.GameId).HasName("PK__Gamifica__2AB897DDAB7BA769");

            entity.ToTable("Gamification");

            entity.Property(e => e.GameId).HasColumnName("GameID");
            entity.Property(e => e.BadgesEarned).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Gamifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Gamificat__UserI__45F365D3");
        });

        modelBuilder.Entity<ProgressTracking>(entity =>
        {
            entity.HasKey(e => e.ProgressId).HasName("PK__Progress__BAE29C85206DF6BA");

            entity.ToTable("ProgressTracking");

            entity.Property(e => e.ProgressId).HasColumnName("ProgressID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.WorkoutId).HasColumnName("WorkoutID");

            entity.HasOne(d => d.User).WithMany(p => p.ProgressTrackings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProgressT__UserI__3D5E1FD2");

            entity.HasOne(d => d.Workout).WithMany(p => p.ProgressTrackings)
                .HasForeignKey(d => d.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__ProgressT__Worko__3E52440B");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACA48124C2");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534663A7F65").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.MidName).HasMaxLength(50);
        });

        modelBuilder.Entity<Voice>(entity =>
        {
            entity.HasKey(e => e.VoiceId).HasName("PK__Voices__D870D5876A62D643");

            entity.Property(e => e.VoiceId).HasColumnName("VoiceID");
            entity.Property(e => e.VoiceType).HasMaxLength(50);
            entity.Property(e => e.WorkoutId).HasColumnName("WorkoutID");

            entity.HasOne(d => d.Workout).WithMany(p => p.Voices)
                .HasForeignKey(d => d.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Voices__WorkoutI__4316F928");
        });

        modelBuilder.Entity<VrModule>(entity =>
        {
            entity.HasKey(e => e.Vrid).HasName("PK__VR_Modul__4D45925BAFE8E79D");

            entity.ToTable("VR_Module");

            entity.Property(e => e.Vrid).HasColumnName("VRID");
            entity.Property(e => e.Environment).HasMaxLength(255);
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(e => e.WorkoutId).HasName("PK__Workouts__E1C42A214CE9ADC8");

            entity.Property(e => e.WorkoutId).HasColumnName("WorkoutID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
