using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ProductCatalogApi.Data;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CatalogContext>
    {
        public CatalogContext CreateDbContext(string[] args)
        {
            //throw new System.NotImplementedException();
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<CatalogContext>();

            var connectionString = configuration.GetConnectionString("MSSQLConnection");

            builder.UseSqlServer(connectionString);

            return new CatalogContext(builder.Options);
        }
       
    }