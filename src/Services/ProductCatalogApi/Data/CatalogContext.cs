using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalogApi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalogApi.Data
{
    public class CatalogContext:DbContext
    {
        
        public CatalogContext(DbContextOptions options):base(options)
        {

        }

        
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CatalogBrand>(ConfigureCatalogBrand);
            builder.Entity<CatalogType>(ConfigureCatalogType);
            builder.Entity<CatalogItem>(ConfigureCatalogItem);
        }

        private void ConfigureCatalogItem(EntityTypeBuilder<CatalogItem> builder)
        {
            builder.ToTable("Catalog");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("catalog_hilo")
                .IsRequired(true);
            builder.Property(c => c.Name)
                    .IsRequired(true)
                    .HasMaxLength(50);
            builder.Property(c => c.Price)
                .IsRequired(true);
            builder.Property(c => c.PictureUrl)
                .IsRequired(false);
            builder.HasOne(c => c.CatalogBrand)
                .WithMany()
                .HasForeignKey(c => c.CatalogBrandId);

            builder.HasOne(c => c.CatalogType)
               .WithMany()
               .HasForeignKey(c => c.CatalogTypeId);

        }

        private void ConfigureCatalogType(EntityTypeBuilder<CatalogType> builder)
        {
            builder.ToTable("CatalogType");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("catalog_type_hilo")
                .IsRequired();
            builder.Property(c => c.Type)
                .IsRequired()
                .HasMaxLength(100);
        }

        private void ConfigureCatalogBrand(EntityTypeBuilder<CatalogBrand> builder)
        {
            builder.ToTable("CatalogBrand");
            builder.Property(c => c.Id)
                .ForSqlServerUseSequenceHiLo("catalog_brand_hilo")
                .IsRequired();
            builder.Property(c => c.Brand)
                .IsRequired()
                .HasMaxLength(100);
        }
        
        public virtual void Save()  
        {  
        base.SaveChanges();  
        }  

        public string UserProvider  
        {  
            get  
            {  
                if (!string.IsNullOrEmpty(WindowsIdentity.GetCurrent().Name))  
                return WindowsIdentity.GetCurrent().Name.Split('\\')[1];  
                return string.Empty;  
            }  
        }  
    
        public Func<DateTime> TimestampProvider { get; set; } = ()  => DateTime.UtcNow;  
        public override int SaveChanges()  
        {  
        TrackChanges();  
        return base.SaveChanges();  
        }  
        
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())  
        {  
        TrackChanges();  
        return await base.SaveChangesAsync(cancellationToken);  
        }  
    
        private void TrackChanges()  
        {  
            foreach (var entry in this.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))  
            {  
                if (entry.Entity is BaseEntity)  
                {  
                    var auditable = entry.Entity as BaseEntity;  
                    if (entry.State == EntityState.Added)  
                    {  
                        auditable.CreatedUser = UserProvider;
                        auditable.CreatedDate = TimestampProvider();  
                        auditable.ModifiedDate = TimestampProvider();  
                    }  
                    else  
                    {  
                        auditable.ModifiedUser = UserProvider;  
                        auditable.ModifiedDate = TimestampProvider();  
                    }  
                }  
            }  
        }  
    }
}
