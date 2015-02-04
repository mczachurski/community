using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Migrations.History;

namespace SunLine.Community.Repositories
{
    public class ExHistoryContext : HistoryContext
    {
        public ExHistoryContext(DbConnection dbConnection, string defaultSchema)
            : base(dbConnection, defaultSchema)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HistoryRow>().ToTable(tableName: "MigrationHistories");
        }
    } 
}
