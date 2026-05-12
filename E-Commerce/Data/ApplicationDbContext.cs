using E_Commerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel;
using System.Reflection.Emit;

namespace E_Commerce.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(MainEntity).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(MainEntity))
                {
                    builder.Entity(entityType.ClrType)
                        .Property("IsDeleted")
                        .HasDefaultValue(false);

                    builder.Entity(entityType.ClrType)
                        .Property("CreationDate")
                        .HasDefaultValueSql("GETDATE()")
                        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                }
            }

        }
    }
}
