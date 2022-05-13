using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            Assembly currentAssembly = typeof(ApplicationDbContext).Assembly;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddUserSecrets(currentAssembly)
                .Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DBCS");
            builder.UseNpgsql(connectionString, o =>
            {
                o.MigrationsAssembly(currentAssembly.GetName().Name);
                o.MigrationsHistoryTable("__efmigrationshistory", "public");
            })
            .UseSnakeCaseNamingConvention()
            .ReplaceService<IHistoryRepository, LoweredCaseMigrationHistoryRepository>();

            return new ApplicationDbContext(builder.Options);
        }
    }
}