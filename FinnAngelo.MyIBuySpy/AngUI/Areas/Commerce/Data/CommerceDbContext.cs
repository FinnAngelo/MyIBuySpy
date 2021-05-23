using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Models;

#nullable disable

namespace FinnAngelo.MyIBuySpy.AngUI.Areas.Commerce.Data
{
    public partial class CommerceDbContext : DbContext
    {
        public CommerceDbContext()
        {
        }

        public CommerceDbContext(DbContextOptions<CommerceDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<VewOrderDetail> VewOrderDetails { get; set; }
        public virtual DbSet<ViewAlsoPurchased> ViewAlsoPurchaseds { get; set; }
        public virtual DbSet<ViewCart> ViewCarts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //TODO: Find out why named connection isnt working for generate controller
                //optionsBuilder.UseSqlServer("name=CommerceConnection");
                optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Commerce;User ID=sa;Password=Password_01;Connect Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.CategoryId)
                    .IsClustered(false);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CategoryName).HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .IsClustered(false);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CustomerName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ShipDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitCost).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_Order_OrderDetails");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .IsClustered(false);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.Description).HasMaxLength(3800);

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.Property(e => e.ModelNumber).HasMaxLength(50);

                entity.Property(e => e.ProductImage).HasMaxLength(50);

                entity.Property(e => e.UnitCost).HasColumnType("money");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Products_Categories");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.ReviewId).HasColumnName("ReviewID");

                entity.Property(e => e.Comments).HasMaxLength(3850);

                entity.Property(e => e.CustomerEmail).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Reviews_Products");
            });

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.RecordId)
                    .IsClustered(false);

                entity.ToTable("ShoppingCart");

                entity.HasIndex(e => new { e.CartId, e.ProductId }, "IX_ShoppingCart");

                entity.Property(e => e.RecordId).HasColumnName("RecordID");

                entity.Property(e => e.CartId)
                    .HasMaxLength(50)
                    .HasColumnName("CartID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Quantity).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShoppingCarts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShoppingCart_Products");
            });

            modelBuilder.Entity<VewOrderDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("VewOrderDetails");

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.Property(e => e.ModelNumber).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitCost).HasColumnType("money");
            });

            modelBuilder.Entity<ViewAlsoPurchased>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewAlsoPurchased");

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ProductsProductId).HasColumnName("Products_ProductID");
            });

            modelBuilder.Entity<ViewCart>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("ViewCart");

                entity.Property(e => e.CartId)
                    .HasMaxLength(50)
                    .HasColumnName("CartID");

                entity.Property(e => e.ModelName).HasMaxLength(50);

                entity.Property(e => e.ModelNumber).HasMaxLength(50);

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.UnitCost).HasColumnType("money");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
