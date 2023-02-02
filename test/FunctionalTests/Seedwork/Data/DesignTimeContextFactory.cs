using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Balea.Grantor.EntityFrameworkCore.DbContexts;

namespace FunctionalTests.Seedwork.Data
{
    public class DesignTimeContextFactory : IDesignTimeDbContextFactory<BaleaDbContext>
    {
        public BaleaDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var builder = new DbContextOptionsBuilder<BaleaDbContext>()
                .UseSqlServer(configuration.GetConnectionString("Default"), sqlServerOptions =>
                {
                    sqlServerOptions.MigrationsAssembly(typeof(DesignTimeContextFactory).Assembly.FullName);
                });

            return new BaleaDbContext(builder.Options);
        }
    }
}
