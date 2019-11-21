using System;
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
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail { get; set; }
        public virtual DbSet<SevenElevenShop> SevenElevenShop { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<TaiwanCity> TaiwanCity { get; set; }
        public virtual DbSet<TransportMethod> TransportMethod { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\MSSQLSERVER01;Database=AChome;User Id=sa;password=P@ssw0rd;MultipleActiveResultSets=True;");
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

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderGuid);

                entity.Property(e => e.OrderGuid)
                    .HasMaxLength(50)
                    .ValueGeneratedNever();

                entity.Property(e => e.OrderAccount)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.OrderingTime).HasColumnType("datetime");

                entity.Property(e => e.ReceiverAddress)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReceiverName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ReceiverPhone)
                    .IsRequired()
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => new { e.OrderGuid, e.Seq });

                entity.Property(e => e.OrderGuid).HasMaxLength(50);

                entity.Property(e => e.ProdId)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<SevenElevenShop>(entity =>
            {
                entity.HasKey(e => e.ShopNumber);

                entity.Property(e => e.ShopNumber)
                    .HasMaxLength(10)
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShopName)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasColumnName("ZIP")
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => new { e.Account, e.ProdId, e.SpecId });

                entity.Property(e => e.Account).HasMaxLength(20);

                entity.Property(e => e.ProdId).HasMaxLength(50);

                entity.Property(e => e.AddTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<TaiwanCity>(entity =>
            {
                entity.HasKey(e => new { e.Zip, e.City, e.Area });

                entity.Property(e => e.Zip).HasMaxLength(10);

                entity.Property(e => e.City).HasMaxLength(10);

                entity.Property(e => e.Area).HasMaxLength(10);
            });

            modelBuilder.Entity<TransportMethod>(entity =>
            {
                entity.HasKey(e => e.TransportId);

                entity.Property(e => e.TransportId).ValueGeneratedNever();

                entity.Property(e => e.TransportName)
                    .IsRequired()
                    .HasMaxLength(10);
            });
        }
    }
}
