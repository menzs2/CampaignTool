﻿using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data;

public partial class CampaignToolContext : DbContext
{
    public CampaignToolContext()
    {
    }

    public CampaignToolContext(DbContextOptions<CampaignToolContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<CharCharConnection> CharCharConnections { get; set; }

    public virtual DbSet<CharOrgConnection> CharOrgConnections { get; set; }
    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<Connection> Connections { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // Reads the connection string from configuration (e.g., appsettings.json)
            optionsBuilder.UseNpgsql("Name=CampaignToolDatabase");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Campaign
        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("campaign_pkey");
            entity.ToTable("campaign");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CampaignName).HasMaxLength(100);
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.DescriptionShort).HasMaxLength(200);
            entity.Property(e => e.GmOnlyDescription).HasColumnType("character varying");
        });

        // CharCharConnection
        modelBuilder.Entity<CharCharConnection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("char_char_connection_pkey");
            entity.ToTable("char_char_connection");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GmOnly).HasDefaultValue(false);
            entity.Property(e => e.GmOnlyDescription).HasMaxLength(500);

            entity.HasOne(d => d.CharOne)
                .WithMany(p => p.CharCharConnectionCharOnes)
                .HasForeignKey(d => d.CharOneId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("char_one_id");

            entity.HasOne(d => d.CharTwo)
                .WithMany(p => p.CharCharConnectionCharTwos)
                .HasForeignKey(d => d.CharTwoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("char_two_id");

            entity.HasOne(d => d.Connection)
                .WithMany(p => p.CharCharConnections)
                .HasForeignKey(d => d.ConnectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("connection_fkey");
        });

        // Character
        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("character_pkey");
            entity.ToTable("character");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.CharacterName).HasMaxLength(300);
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.DescriptionShort).HasMaxLength(200);
            entity.Property(e => e.GmOnly).HasDefaultValue(true);
            entity.Property(e => e.GmOnlyDescription).HasColumnType("character varying");
            entity.Property(e => e.State).HasMaxLength(200);

            entity.HasOne(d => d.Campaign)
                .WithMany(p => p.Characters)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("campaign_fkey");
        });

        // Connection
        modelBuilder.Entity<Connection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("connection_pkey");
            entity.ToTable("connection");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.ConnectionName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GmOnly).HasDefaultValue(false);
            entity.Property(e => e.GmOnlyDescription).HasMaxLength(500);

            entity.HasOne(d => d.Campaign)
                .WithMany(p => p.Connections)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("campaign_fkey");
        });

        // Organisation
        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organisation_pkey");
            entity.ToTable("organisation");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Description).HasColumnType("character varying");
            entity.Property(e => e.DescriptionShort).HasMaxLength(200);
            entity.Property(e => e.GmOnly).HasDefaultValue(true);
            entity.Property(e => e.GmOnlyDescription).HasColumnType("character varying");
            entity.Property(e => e.OrganisationName).HasMaxLength(300);
            entity.Property(e => e.State).HasMaxLength(200);

            entity.HasOne(d => d.Campaign)
                .WithMany(p => p.Organisations)
                .HasForeignKey(d => d.CampaignId)
                .HasConstraintName("campaign_fkey");
        });

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");
            entity.ToTable("user");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(150);
            entity.Property(e => e.HasLogin).HasDefaultValue(false);
            entity.Property(e => e.LastName).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(50);
        });

        // UserSetting
        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_setting_pkey");
            entity.ToTable("user_setting");
            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.SameNameWarning).HasDefaultValue(true);
            entity.Property(e => e.SelectLastCampaign).HasDefaultValue(false);

            entity.HasOne(d => d.DefaultCampaign)
                .WithMany(p => p.UserSettings)
                .HasForeignKey(d => d.DefaultCampaignId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("campaign_fkey");

            entity.HasOne(u => u.User)
                .WithOne(us => us.UserSetting)
                .HasForeignKey<UserSetting>(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
