using System;
using System.Collections.Generic;
using CaptureIt.Models;
using Microsoft.EntityFrameworkCore;

namespace CaptureIt.Data;

public partial class CaptureItContext : DbContext
{
    public CaptureItContext()
    {
    }

    public CaptureItContext(DbContextOptions<CaptureItContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Badge> Badges { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<PasswordRecovery> PasswordRecoveries { get; set; }

    public virtual DbSet<Picture> Pictures { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasOne(d => d.Creator).WithMany(p => p.Albums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Albums_Users");

            entity.HasOne(d => d.Event).WithMany(p => p.Albums)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Albums_Events");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasOne(d => d.Picture).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Pictures");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Users");
        });

        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasOne(d => d.Owner).WithMany(p => p.Events)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Events_Users");

            entity.HasMany(d => d.Participants).WithMany(p => p.EventsNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "EventParticipant",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventParticipants_Users"),
                    l => l.HasOne<Event>().WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_EventParticipants_Events"),
                    j =>
                    {
                        j.HasKey("EventId", "ParticipantId");
                        j.ToTable("EventParticipants");
                    });
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasOne(d => d.Picture).WithMany(p => p.Likes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Pictures");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Likes_Users");
        });

        modelBuilder.Entity<PasswordRecovery>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK_PasswordRecoveryRequests");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordRecoveries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PasswordRecovery_Users");
        });

        modelBuilder.Entity<Picture>(entity =>
        {
            entity.HasOne(d => d.Album).WithMany(p => p.Pictures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pictures_Albums");

            entity.HasOne(d => d.Author).WithMany(p => p.Pictures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pictures_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasMany(d => d.Badges).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserBadge",
                    r => r.HasOne<Badge>().WithMany()
                        .HasForeignKey("BadgeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserBadges_Badges"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_UserBadges_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "BadgeId");
                        j.ToTable("UserBadges");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
