using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Velzon.Models;

public partial class VelzonContext : DbContext
{
    public VelzonContext()
    {
    }

    public VelzonContext(DbContextOptions<VelzonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AddressDetail> AddressDetails { get; set; }

    public virtual DbSet<CompanyDocument> CompanyDocuments { get; set; }

    public virtual DbSet<PersonalInfo> PersonalInfos { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AddressDetail>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PK__AddressD__091C2AFBEB81C6A2");

            entity.ToTable("AddressDetail");

            entity.Property(e => e.City)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentAddress)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.PermanentAddress)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.StateProvince)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ZipPostalCode)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.AddressDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AddressDetail_Users");
        });

        modelBuilder.Entity<CompanyDocument>(entity =>
        {
            entity.HasKey(e => e.DocumentId).HasName("PK__CompanyD__1ABEEF0F45272300");

            entity.Property(e => e.CertificateOfIncorporation)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MemorandumAndArticles)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ShareholdingStructure)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.TaxIdentificationNumber)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.CompanyDocuments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CompanyDocuments_Users");
        });

        modelBuilder.Entity<PersonalInfo>(entity =>
        {
            entity.HasKey(e => e.PersonalInfoId).HasName("PK__Personal__EA7BF0E42CA24668");

            entity.ToTable("PersonalInfo");

            entity.Property(e => e.ContactNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Nationality)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ParentName)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.PersonalInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalInfo_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C575D6EA0");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105343FB4D2E8").IsUnique();

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
