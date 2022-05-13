using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Data
{
    public class LoweredCaseMigrationHistoryRepository :
#pragma warning disable EF1001
        Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal.NpgsqlHistoryRepository
#pragma warning restore EF1001
    {
#pragma warning disable EF1001
        public LoweredCaseMigrationHistoryRepository(HistoryRepositoryDependencies dependencies) : base(dependencies)
#pragma warning restore EF1001
        {
        }
        protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
        {
            base.ConfigureTable(history);
            history.Property(h => h.MigrationId).HasColumnName("migrationid");
            history.Property(h => h.ProductVersion).HasColumnName("productversion");
        }
    }
}