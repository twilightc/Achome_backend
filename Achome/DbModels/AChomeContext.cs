﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Achome.DbModels
{
    public partial class AChomeContext : DbContext
    {
        public AChomeContext()
        {
        }

        public AChomeContext(DbContextOptions<AChomeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<CategoryDetail> CategoryDetail { get; set; }
        public virtual DbSet<Merchandise> Merchandise { get; set; }
        public virtual DbSet<MerchandiseQa> MerchandiseQa { get; set; }
        public virtual DbSet<MerchandiseSpec> MerchandiseSpec { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=192.168.20.11;Database=AChome;User Id=sa;password=P@ssw0rd;MultipleActiveResultSets=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.AccountName)
                    .HasName("PK_Account_1");

                entity.Property(e => e.AccountName)
                    .HasMaxLength(20)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Registertime).HasColumnType("datetime");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Cid);

                entity.Property(e => e.Cid)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Cname)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<CategoryDetail>(entity =>
            {
                entity.HasKey(e => new { e.Cid, e.DetailId });

                entity.Property(e => e.Cid).HasMaxLength(10);

                entity.Property(e => e.DetailId).HasMaxLength(10);

                entity.Property(e => e.DetailName)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<Merchandise>(entity =>
            {
                entity.Property(e => e.MerchandiseId)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.CategoryDetailId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.CategoryId)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ImagePath).HasMaxLength(50);

                entity.Property(e => e.MerchandiseTitle)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MerhandiseContent).IsRequired();

                entity.Property(e => e.OwnerAccount)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<MerchandiseQa>(entity =>
            {
                entity.HasKey(e => new { e.MerchandiseId, e.Seq });

                entity.ToTable("MerchandiseQA");

                entity.Property(e => e.MerchandiseId).HasMaxLength(50);

                entity.Property(e => e.AnswerTime).HasColumnType("date");

                entity.Property(e => e.AskingTime).HasColumnType("date");

                entity.Property(e => e.Question).IsRequired();

                entity.Property(e => e.QuestionAccount)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<MerchandiseSpec>(entity =>
            {
                entity.HasKey(e => new { e.MerchandiseId, e.SpecId });

                entity.Property(e => e.MerchandiseId).HasMaxLength(50);

                entity.Property(e => e.Spec1)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Spec2)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.Account, e.ProdId, e.SpecId });

                entity.Property(e => e.Account).HasMaxLength(20);

                entity.Property(e => e.ProdId).HasMaxLength(50);

                entity.Property(e => e.AddTime).HasColumnType("datetime");
            });
        }
    }
}
