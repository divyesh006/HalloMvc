using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HalloDoc_mvc.Entities.DataModels;

public partial class HalloDocContext : DbContext
{
    public HalloDocContext()
    {
    }

    public HalloDocContext(DbContextOptions<HalloDocContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Concierge> Concierges { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestBusiness> RequestBusinesses { get; set; }

    public virtual DbSet<RequestClient> RequestClients { get; set; }

    public virtual DbSet<RequestConcierge> RequestConcierges { get; set; }

    public virtual DbSet<RequestType> RequestTypes { get; set; }

    public virtual DbSet<RequestWiseFile> RequestWiseFiles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=HalloDoc;Username=postgres;Password=password");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AspNetUsers_pkey");
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("AspNetUserRoles_pkey");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId).HasName("Business_pkey");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.BusinessCreatedByNavigations).HasConstraintName("CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.BusinessModifiedByNavigations).HasConstraintName("ModifiedBy");

            entity.HasOne(d => d.Region).WithMany(p => p.Businesses).HasConstraintName("RegionId");
        });

        modelBuilder.Entity<Concierge>(entity =>
        {
            entity.HasKey(e => e.ConciergeId).HasName("Concierge_pkey");

            entity.HasOne(d => d.Region).WithMany(p => p.Concierges).HasConstraintName("RegionId");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("Region_pkey");

            entity.Property(e => e.RegionId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("Request_pkey");

            entity.Property(e => e.CompletedByPhysician).HasDefaultValueSql("'0'::\"bit\"");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'::\"bit\"");
            entity.Property(e => e.IsMobile).HasDefaultValueSql("'0'::\"bit\"");
            entity.Property(e => e.IsUrgentEmailSent).HasDefaultValueSql("'0'::\"bit\"");

            entity.HasOne(d => d.RequestType).WithMany(p => p.Requests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RequestId");

            entity.HasOne(d => d.User).WithMany(p => p.Requests).HasConstraintName("UserId");
        });

        modelBuilder.Entity<RequestBusiness>(entity =>
        {
            entity.HasKey(e => e.RequestBusinessId).HasName("RequestBusiness_pkey");

            entity.HasOne(d => d.Business).WithMany(p => p.RequestBusinesses).HasConstraintName("BusinessId");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestBusinesses).HasConstraintName("RequestId");
        });

        modelBuilder.Entity<RequestClient>(entity =>
        {
            entity.HasKey(e => e.RequestClientId).HasName("RequestClient_pkey");

            entity.HasOne(d => d.Region).WithMany(p => p.RequestClients).HasConstraintName("RegionId");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestClients).HasConstraintName("RequestId");
        });

        modelBuilder.Entity<RequestConcierge>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RequestConcierge_pkey");

            entity.HasOne(d => d.Concierge).WithMany(p => p.RequestConcierges).HasConstraintName("ConciergeId");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestConcierges).HasConstraintName("RequestId");
        });

        modelBuilder.Entity<RequestType>(entity =>
        {
            entity.HasKey(e => e.RequestTypeId).HasName("RequestType_pkey");

            entity.Property(e => e.RequestTypeId).ValueGeneratedNever();
        });

        modelBuilder.Entity<RequestWiseFile>(entity =>
        {
            entity.HasKey(e => e.RequestWiseFileId).HasName("RequestWiseFile_pkey");

            entity.Property(e => e.IsCompensation).HasDefaultValueSql("false");
            entity.Property(e => e.IsDeleted).HasDefaultValueSql("false");
            entity.Property(e => e.IsFinalize).HasDefaultValueSql("false");
            entity.Property(e => e.IsFrontSide).HasDefaultValueSql("false");
            entity.Property(e => e.IsPatientRecords).HasDefaultValueSql("false");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestWiseFiles).HasConstraintName("RequestId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("User_pkey");

            entity.Property(e => e.IsDeleted).HasDefaultValueSql("'0'::\"bit\"");
            entity.Property(e => e.IsMobile).HasDefaultValueSql("'0'::\"bit\"");
            entity.Property(e => e.IsRequestWithEmail).HasDefaultValueSql("'0'::\"bit\"");

            entity.HasOne(d => d.Region).WithMany(p => p.Users).HasConstraintName("RegionID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
