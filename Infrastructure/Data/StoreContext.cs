
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductBrandEntity> ProductBrands { get; set; }
        public DbSet<ProductTypeEntity> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ProductEntity>().Property(p => p.Id).IsRequired();
            builder.Entity<ProductEntity>().Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Entity<ProductEntity>().Property(p => p.Description).IsRequired();
            builder.Entity<ProductEntity>().Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Entity<ProductEntity>().Property(p => p.PictureUrl).IsRequired();
            builder.Entity<ProductEntity>().HasOne(p => p.Brand).WithMany().HasForeignKey(p => p.BrandId);
            builder.Entity<ProductEntity>().HasOne(p => p.Type).WithMany().HasForeignKey(p => p.TypeId);
        }
    }
}