using ClothingStore.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClothingStore.DAL.Db
{
    public class ClothingStoreContext : DbContext
    {
        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<ProductSupplier> ProductSuppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1:1 relationship - Product to ProductDetail
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductDetail)
                .WithOne(d => d.Product)
                .HasForeignKey<ProductDetail>(d => d.ProductId);

            // 1:N relationship - Category to Products
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId);

            // M:N relationship - Product to Supplier through ProductSupplier
            modelBuilder.Entity<ProductSupplier>()
                .HasKey(ps => new { ps.ProductId, ps.SupplierId });

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSuppliers)
                .HasForeignKey(ps => ps.ProductId);

            modelBuilder.Entity<ProductSupplier>()
                .HasOne(ps => ps.Supplier)
                .WithMany(s => s.ProductSuppliers)
                .HasForeignKey(ps => ps.SupplierId);

            // Indexes
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.Name);

            modelBuilder.Entity<Product>()
                .HasIndex(p => p.SKU)
                .IsUnique();

            // Constraints
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ProductDetail>()
                .Property(pd => pd.StockQuantity)
                .HasDefaultValue(0);

            modelBuilder.Entity<Product>()
                .Property(p => p.IsActive)
                .HasDefaultValue(true);

            modelBuilder.Entity<Category>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
        }
    }
}
